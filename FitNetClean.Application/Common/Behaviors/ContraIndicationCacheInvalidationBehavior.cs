using FitNetClean.Application.Common.Cache;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.Features.Shared.Commands;
using FitNetClean.Domain.Entities;
using MediatR;

namespace FitNetClean.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior that invalidates cache when ContraIndication entities are modified
/// </summary>
public class ContraIndicationCacheInvalidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public ContraIndicationCacheInvalidationBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (ShouldInvalidateCache(request))
        {
            await InvalidateContraIndicationCache(request, cancellationToken);
        }

        return response;
    }

    private static bool ShouldInvalidateCache(TRequest request)
    {
        // Check if this is a command that modifies ContraIndication or related entities
        return request switch
        {
            CreateCommand<ContraIndication> => true,
            UpdateCommand<ContraIndication> => true,
            DeleteCommand<ContraIndication> => true,
            CreateCommand<Equipment> => true,
            UpdateCommand<Equipment> => true,
            DeleteCommand<Equipment> => true,
            CreateCommand<Exercise> => true,
            UpdateCommand<Exercise> => true,
            DeleteCommand<Exercise> => true,
            _ => false
        };
    }

    private async Task InvalidateContraIndicationCache(TRequest request, CancellationToken ct)
    {
        // Invalidate all ContraIndication related caches
        await _cache.RemoveByPatternAsync($"{CacheKeys.ContraIndicationPrefix}:*", ct);
        
        // Invalidate Equipment and Exercise caches as they contain ContraIndications
        await _cache.RemoveByPatternAsync($"{CacheKeys.EquipmentPrefix}:*", ct);
        await _cache.RemoveByPatternAsync($"{CacheKeys.ExercisePrefix}:*", ct);
    }
}
