using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Specifications;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Workouts.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Queries;

public record GetWorkoutDetailQuery(long Id) : IRequest<WorkoutDetailDto?>;

public class GetWorkoutDetailHandler(IFitnetContext context)
    : IRequestHandler<GetWorkoutDetailQuery, WorkoutDetailDto?>
{
    public async Task<WorkoutDetailDto?> Handle(GetWorkoutDetailQuery request, CancellationToken ct)
    {
        var specification = new WorkoutWithDetailsSpecification(request.Id);

        var query = context.Workout.AsNoTracking();
        var projectedQuery = SpecificationEvaluator.GetQuery(query, specification);

        var result = await projectedQuery.FirstOrDefaultAsync(ct);

        return result;
    }
}
