using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Common.Specifications;
using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Features.ContraIndications.Specifications;

public class ContraIndicationWithRelatedSpecification : BaseSpecification<ContraIndication, ContraIndicationRelatedDto>
{
    public ContraIndicationWithRelatedSpecification(long contraIndicationId) 
        : base(ci => ci.Id == contraIndicationId)
    {
        AddInclude("ExerciseList");
        AddInclude("EquipmentList.Category");

        ApplySelector(ci => new ContraIndicationRelatedDto(
            ci.Id,
            ci.Name,
            ci.ExerciseList
                .OrderBy(e => e.Name)
                .Select(e => new ExerciseDto(
                    e.Id,
                    e.Name,
                    e.Repetition,
                    e.WorkoutId,
                    e.WorkoutGroupId,
                    e.EquipmentId
                ))
                .ToList(),
            ci.EquipmentList
                .OrderBy(eq => eq.Name)
                .Select(eq => new EquipmentDto(
                    eq.Id,
                    eq.Name,
                    eq.CategoryId,
                    eq.Category.Name
                ))
                .ToList()
        ));
    }
}
