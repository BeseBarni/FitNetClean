using FitNetClean.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace FitNetClean.Infrastructure.Services;

/// <summary>
/// In-memory cache service implementation using IMemoryCache
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, byte> _keys;
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(30);

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
        _keys = new ConcurrentDictionary<string, byte>();
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
    {
        var value = _cache.Get<T>(key);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default) where T : class
    {
        var cacheExpiration = expiration ?? DefaultExpiration;
        
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(cacheExpiration)
            .RegisterPostEvictionCallback((k, v, r, s) =>
            {
                _keys.TryRemove(k.ToString()!, out _);
            });

        _cache.Set(key, value, options);
        _keys.TryAdd(key, 0);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        _cache.Remove(key);
        _keys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern, CancellationToken ct = default)
    {
        // Convert wildcard pattern to regex
        var regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
        var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

        var keysToRemove = _keys.Keys.Where(k => regex.IsMatch(k)).ToList();

        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }

    public async Task<T> GetOrCreateAsync<T>(
        string key, 
        Func<Task<T>> factory, 
        TimeSpan? expiration = null, 
        CancellationToken ct = default) where T : class
    {
        if (_cache.TryGetValue<T>(key, out var cached) && cached != null)
        {
            return cached;
        }

        var value = await factory();
        await SetAsync(key, value, expiration, ct);

        return value;
    }
}
