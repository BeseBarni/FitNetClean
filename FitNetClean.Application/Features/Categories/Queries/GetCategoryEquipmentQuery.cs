using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Specifications;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Categories.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Categories.Queries;

public record GetCategoryEquipmentQuery(long CategoryId) : IRequest<CategoryEquipmentDto?>;

public class GetCategoryEquipmentHandler(IFitnetContext context)
    : IRequestHandler<GetCategoryEquipmentQuery, CategoryEquipmentDto?>
{
    public async Task<CategoryEquipmentDto?> Handle(GetCategoryEquipmentQuery request, CancellationToken ct)
    {
        var specification = new CategoryWithEquipmentSpecification(request.CategoryId);

        var query = context.Category.AsNoTracking();
        var projectedQuery = SpecificationEvaluator.GetQuery(query, specification);

        return await projectedQuery.FirstOrDefaultAsync(ct);
    }
}
