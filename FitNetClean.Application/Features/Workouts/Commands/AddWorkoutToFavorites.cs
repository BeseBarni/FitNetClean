using FitNetClean.Application.Common;
using FitNetClean.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Commands;

public record AddWorkoutToFavoritesCommand(long UserId, long WorkoutId) : IRequest<bool>;

public class AddWorkoutToFavoritesHandler(IFitnetContext context) 
    : IRequestHandler<AddWorkoutToFavoritesCommand, bool>
{
    public async Task<bool> Handle(AddWorkoutToFavoritesCommand request, CancellationToken ct)
    {
        var workout = await context.Workout
            .FirstOrDefaultAsync(w => w.Id == request.WorkoutId, ct);

        if (workout == null)
            return false;

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return false;

        var existingFavorite = await context.FavoriteWorkout
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(fw => fw.UserId == request.UserId && fw.WorkoutId == request.WorkoutId, ct);

        if (existingFavorite != null)
        {
            if (existingFavorite.IsDeleted)
            {
                existingFavorite.IsDeleted = false;
                existingFavorite.FavoritedAt = DateTime.UtcNow;
                await context.SaveChangesAsync(ct);
            }
            return true;
        }

        var favoriteWorkout = new FavoriteWorkout
        {
            UserId = request.UserId,
            WorkoutId = request.WorkoutId,
            FavoritedAt = DateTime.UtcNow
        };

        context.FavoriteWorkout.Add(favoriteWorkout);
        await context.SaveChangesAsync(ct);

        return true;
    }
}
