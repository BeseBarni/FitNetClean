using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Common.Specifications;
using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Features.Exercises.Specifications;

public class ExerciseWorkoutProgramsSpecification : BaseSpecification<Exercise, ExerciseWorkoutProgramsDto>
{
    public ExerciseWorkoutProgramsSpecification(long exerciseId)
        : base(e => e.Id == exerciseId)
    {
        AddInclude("Workout.ExerciseList.ContraIndicationList");
        AddInclude("Workout.ExerciseList.Equipment.ContraIndicationList");

        ApplySelector(e => new ExerciseWorkoutProgramsDto(
            e.Id,
            e.Name,
            new List<WorkoutProgramSummaryDto>
            {
                new WorkoutProgramSummaryDto(
                    e.Workout.Id,
                    e.Workout.CodeName,
                    e.Workout.Title,
                    e.Workout.Description,
                    e.Workout.WarmupDurationMinutes,
                    e.Workout.MainWorkoutDurationMinutes,
                    e.Workout.TotalDurationMinutes,
                    e.Workout.IsDeleted,
                    e.Workout.ExerciseList
                        .Where(ex => !ex.IsDeleted) // ✅ Filter deleted exercises
                        .SelectMany(ex => ex.ContraIndicationList.Where(ci => !ci.IsDeleted)) // ✅ Filter deleted CI
                        .Union(
                            e.Workout.ExerciseList
                                .Where(ex => !ex.IsDeleted && ex.Equipment != null && !ex.Equipment.IsDeleted) // ✅ Filter deleted
                                .SelectMany(ex => ex.Equipment!.ContraIndicationList.Where(ci => !ci.IsDeleted)) // ✅ Filter deleted CI
                        )
                        .Distinct()
                        .OrderBy(ci => ci.Name)
                        .Select(ci => new ContraIndicationDetailDto(ci.Id, ci.Name))
                        .ToList()
                )
            }
        ));
    }
}
