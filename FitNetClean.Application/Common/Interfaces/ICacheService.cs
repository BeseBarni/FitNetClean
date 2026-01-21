namespace FitNetClean.Application.Common.Interfaces;

/// <summary>
/// Generic cache service interface for caching application data
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Get value from cache by key
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class;

    /// <summary>
    /// Set value in cache with optional expiration
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default) where T : class;

    /// <summary>
    /// Remove value from cache by key
    /// </summary>
    Task RemoveAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Remove all values matching a pattern
    /// </summary>
    Task RemoveByPatternAsync(string pattern, CancellationToken ct = default);

    /// <summary>
    /// Get or create cached value
    /// </summary>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken ct = default) where T : class;
}
