using FitNetClean.Application.Common;
using FitNetClean.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Exercises.Queries;

public record GetExerciseWorkoutProgramsQuery(long ExerciseId) : IRequest<ExerciseWorkoutProgramsDto?>;

public class GetExerciseWorkoutProgramsHandler(IFitnetContext context)
    : IRequestHandler<GetExerciseWorkoutProgramsQuery, ExerciseWorkoutProgramsDto?>
{
    public async Task<ExerciseWorkoutProgramsDto?> Handle(GetExerciseWorkoutProgramsQuery request, CancellationToken ct)
    {
        var exercise = await context.Exercise
            .IgnoreQueryFilters()
            .Include(e => e.Workout)
                .ThenInclude(w => w.ExerciseList)
                    .ThenInclude(e => e.ContraIndicationList)
            .Include(e => e.Workout)
                .ThenInclude(w => w.ExerciseList)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(eq => eq!.ContraIndicationList)
            .FirstOrDefaultAsync(e => e.Id == request.ExerciseId, ct);

        if (exercise == null)
            return null;

        var contraIndications = exercise.Workout.ExerciseList
            .Where(ex => !ex.IsDeleted) // ✅ Filter deleted exercises (even though IgnoreQueryFilters is used)
            .SelectMany(ex => ex.ContraIndicationList.Where(ci => !ci.IsDeleted)) // ✅ Filter deleted CI
            .Union(
                exercise.Workout.ExerciseList
                    .Where(ex => !ex.IsDeleted && ex.Equipment != null && !ex.Equipment.IsDeleted) // ✅ Filter deleted
                    .SelectMany(ex => ex.Equipment!.ContraIndicationList.Where(ci => !ci.IsDeleted)) // ✅ Filter deleted CI
            )
            .Distinct()
            .OrderBy(ci => ci.Name)
            .Select(ci => new ContraIndicationDetailDto(ci.Id, ci.Name))
            .ToList();

        return new ExerciseWorkoutProgramsDto(
            exercise.Id,
            exercise.Name,
            new List<WorkoutProgramSummaryDto>
            {
                new WorkoutProgramSummaryDto(
                    exercise.Workout.Id,
                    exercise.Workout.CodeName,
                    exercise.Workout.Title,
                    exercise.Workout.Description,
                    exercise.Workout.WarmupDurationMinutes,
                    exercise.Workout.MainWorkoutDurationMinutes,
                    exercise.Workout.TotalDurationMinutes,
                    exercise.Workout.IsDeleted,
                    contraIndications
                )
            }
        );
    }
}
