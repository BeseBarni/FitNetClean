using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.WorkoutGroups.Commands;

public record RemoveExerciseFromWorkoutGroupCommand(long WorkoutGroupId, long ExerciseId) : IRequest<bool>;

public class RemoveExerciseFromWorkoutGroupHandler(IFitnetContext context) 
    : IRequestHandler<RemoveExerciseFromWorkoutGroupCommand, bool>
{
    public async Task<bool> Handle(RemoveExerciseFromWorkoutGroupCommand request, CancellationToken ct)
    {
        var exercise = await context.Exercise
            .FirstOrDefaultAsync(e => e.Id == request.ExerciseId && e.WorkoutGroupId == request.WorkoutGroupId, ct);

        if (exercise == null)
            return false;

        exercise.WorkoutGroupId = null;
        await context.SaveChangesAsync(ct);

        return true;
    }
}
