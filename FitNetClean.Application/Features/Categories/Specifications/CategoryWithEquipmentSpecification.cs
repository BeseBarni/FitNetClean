using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Common.Specifications;
using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Features.Categories.Specifications;

public class CategoryWithEquipmentSpecification : BaseSpecification<Category, CategoryEquipmentDto>
{
    public CategoryWithEquipmentSpecification(long categoryId) 
        : base(c => c.Id == categoryId)
    {
        AddInclude(c => c.EquipmentList);

        ApplySelector(c => new CategoryEquipmentDto(
            c.Id,
            c.Name,
            c.EquipmentList
                .OrderBy(eq => eq.Name)
                .Select(eq => new EquipmentDto(
                    eq.Id,
                    eq.Name,
                    eq.CategoryId,
                    c.Name
                ))
                .ToList()
        ));
    }
}
