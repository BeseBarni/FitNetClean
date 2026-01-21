using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Commands;

public record RemoveWorkoutFromFavoritesCommand(long UserId, long WorkoutId) : IRequest<bool>;

public class RemoveWorkoutFromFavoritesHandler(IFitnetContext context) 
    : IRequestHandler<RemoveWorkoutFromFavoritesCommand, bool>
{
    public async Task<bool> Handle(RemoveWorkoutFromFavoritesCommand request, CancellationToken ct)
    {
        var favoriteWorkout = await context.FavoriteWorkout
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(fw => fw.UserId == request.UserId && fw.WorkoutId == request.WorkoutId, ct);

        if (favoriteWorkout == null)
            return false;

        if (favoriteWorkout.IsDeleted)
            return false;

        favoriteWorkout.IsDeleted = true;
        await context.SaveChangesAsync(ct);

        return true;
    }
}
