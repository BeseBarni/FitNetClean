using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Specifications;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.ContraIndications.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.ContraIndications.Queries;

public record GetContraIndicationRelatedQuery(long ContraIndicationId) : IRequest<ContraIndicationRelatedDto?>;

public class GetContraIndicationRelatedHandler(IFitnetContext context)
    : IRequestHandler<GetContraIndicationRelatedQuery, ContraIndicationRelatedDto?>
{
    public async Task<ContraIndicationRelatedDto?> Handle(GetContraIndicationRelatedQuery request, CancellationToken ct)
    {
        var specification = new ContraIndicationWithRelatedSpecification(request.ContraIndicationId);

        var query = context.ContraIndication.AsNoTracking();
        var projectedQuery = SpecificationEvaluator.GetQuery(query, specification);

        return await projectedQuery.FirstOrDefaultAsync(ct);
    }
}
