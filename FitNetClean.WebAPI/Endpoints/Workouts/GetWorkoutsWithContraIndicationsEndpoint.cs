using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Workouts;
using FitNetClean.Application.Features.Workouts.Queries;
using FitNetClean.WebAPI.Extensions;
using MediatR;

namespace FitNetClean.WebAPI.Endpoints.Workouts;

public class GetWorkoutsWithContraIndicationsEndpoint(IMediator mediator) 
    : Endpoint<GetWorkoutsWithContraIndicationsRequest, ApiResponse<List<WorkoutDto>>>
{
    public override void Configure()
    {
        Get("/workouts/with-contraindications");
    }

    public override async Task HandleAsync(GetWorkoutsWithContraIndicationsRequest req, CancellationToken ct)
    {
        var query = new GetWorkoutsForUserQuery(req.UserId, req.IncludeDeleted);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<List<WorkoutDto>>.Success(result, requestId);
    }
}
