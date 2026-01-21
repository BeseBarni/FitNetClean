using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Cache;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.Common.Specifications;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.ContraIndications.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.ContraIndications.Queries;

public record GetContraIndicationRelatedQuery(long ContraIndicationId) : IRequest<ContraIndicationRelatedDto?>;

public class GetContraIndicationRelatedHandler : IRequestHandler<GetContraIndicationRelatedQuery, ContraIndicationRelatedDto?>
{
    private readonly IFitnetContext _context;
    private readonly ICacheService _cache;

    public GetContraIndicationRelatedHandler(IFitnetContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ContraIndicationRelatedDto?> Handle(GetContraIndicationRelatedQuery request, CancellationToken ct)
    {
        var cacheKey = CacheKeys.ContraIndicationRelated(request.ContraIndicationId);

        var result = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FetchFromDatabase(request.ContraIndicationId, ct),
            TimeSpan.FromMinutes(30),
            ct
        );

        return result;
    }

    private async Task<ContraIndicationRelatedDto?> FetchFromDatabase(long contraIndicationId, CancellationToken ct)
    {
        var specification = new ContraIndicationWithRelatedSpecification(contraIndicationId);

        var query = _context.ContraIndication.AsNoTracking();
        var projectedQuery = SpecificationEvaluator.GetQuery(query, specification);

        return await projectedQuery.FirstOrDefaultAsync(ct);
    }
}
