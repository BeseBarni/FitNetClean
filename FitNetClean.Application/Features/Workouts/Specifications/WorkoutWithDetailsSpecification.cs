using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Common.Specifications;
using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Features.Workouts.Specifications;

public class WorkoutWithDetailsSpecification : BaseSpecification<Workout, WorkoutDetailDto>
{
    public WorkoutWithDetailsSpecification(long workoutId) 
        : base(w => w.Id == workoutId)
    {
        AddInclude("WorkoutGroupList.ExerciseList.Equipment.Category");
        AddInclude("WorkoutGroupList.ExerciseList.ContraIndicationList");

        ApplySelector(w => new WorkoutDetailDto(
            w.Id,
            w.CodeName,
            w.Title,
            w.Description,
            w.WarmupDurationMinutes,
            w.MainWorkoutDurationMinutes,
            w.TotalDurationMinutes,
            w.WorkoutGroupList
                .OrderBy(wg => wg.Id)
                .Select(wg => new WorkoutGroupDetailDto(
                    wg.Id,
                    wg.Title,
                    wg.ExerciseList
                        .OrderBy(e => e.Id)
                        .Select(e => new ExerciseDetailDto(
                            e.Id,
                            e.Name,
                            e.Repetition.ToString(),
                            e.Equipment != null
                                ? new EquipmentDetailDto(
                                    e.Equipment.Id,
                                    e.Equipment.Name,
                                    e.Equipment.Category.Name)
                                : null,
                            e.ContraIndicationList
                                .OrderBy(ci => ci.Name)
                                .Select(ci => new ContraIndicationDetailDto(ci.Id, ci.Name))
                                .ToList()
                        ))
                        .ToList()
                ))
                .ToList()
        ));
    }
}
