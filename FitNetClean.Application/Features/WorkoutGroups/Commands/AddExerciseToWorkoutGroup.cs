using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.WorkoutGroups.Commands;

public record AddExerciseToWorkoutGroupCommand(long WorkoutGroupId, long ExerciseId) : IRequest<bool>;

public class AddExerciseToWorkoutGroupHandler(IFitnetContext context)
    : IRequestHandler<AddExerciseToWorkoutGroupCommand, bool>
{
    public async Task<bool> Handle(AddExerciseToWorkoutGroupCommand request, CancellationToken ct)
    {
        var workoutGroup = await context.WorkoutGroup
            .FirstOrDefaultAsync(wg => wg.Id == request.WorkoutGroupId, ct);

        if (workoutGroup == null)
            return false;

        var exercise = await context.Exercise
            .FirstOrDefaultAsync(e => e.Id == request.ExerciseId, ct);

        if (exercise == null)
            return false;

        if (exercise.WorkoutId != workoutGroup.WorkoutId)
            return false;

        exercise.WorkoutGroupId = request.WorkoutGroupId;
        await context.SaveChangesAsync(ct);

        return true;
    }
}
