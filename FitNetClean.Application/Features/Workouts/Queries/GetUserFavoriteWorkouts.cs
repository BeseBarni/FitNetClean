using AutoMapper;
using FitNetClean.Application.Common;
using FitNetClean.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Queries;

public record GetUserFavoriteWorkoutsQuery(long UserId) : IRequest<List<WorkoutDto>>;

public class GetUserFavoriteWorkoutsHandler(IFitnetContext context, IMapper mapper) 
    : IRequestHandler<GetUserFavoriteWorkoutsQuery, List<WorkoutDto>>
{
    public async Task<List<WorkoutDto>> Handle(GetUserFavoriteWorkoutsQuery request, CancellationToken ct)
    {
        var favoriteWorkouts = await context.FavoriteWorkout
            .Where(fw => fw.UserId == request.UserId)
            .Include(fw => fw.Workout)
            .Select(fw => fw.Workout)
            .ToListAsync(ct);

        return mapper.Map<List<WorkoutDto>>(favoriteWorkouts);
    }
}
