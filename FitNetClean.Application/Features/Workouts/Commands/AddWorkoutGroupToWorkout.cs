using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Commands;

public record AddWorkoutGroupToWorkoutCommand(long WorkoutId, long WorkoutGroupId) : IRequest<bool>;

public class AddWorkoutGroupToWorkoutHandler(IFitnetContext context)
    : IRequestHandler<AddWorkoutGroupToWorkoutCommand, bool>
{
    public async Task<bool> Handle(AddWorkoutGroupToWorkoutCommand request, CancellationToken ct)
    {
        var workout = await context.Workout
            .FirstOrDefaultAsync(w => w.Id == request.WorkoutId, ct);

        if (workout == null)
            return false;

        var workoutGroup = await context.WorkoutGroup
            .FirstOrDefaultAsync(wg => wg.Id == request.WorkoutGroupId, ct);

        if (workoutGroup == null)
            return false;

        workoutGroup.WorkoutId = request.WorkoutId;
        await context.SaveChangesAsync(ct);

        return true;
    }
}
