using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.Workouts.Queries;
using MediatR;

namespace FitNetClean.Application.Features.Workouts;

public record GetWorkoutsWithContraIndicationsRequest
{
    public long? UserId { get; init; }
    public bool IncludeDeleted { get; init; } = false;
}

public class GetWorkoutsWithContraIndicationsEndpoint : Endpoint<GetWorkoutsWithContraIndicationsRequest, ApiResponse<List<WorkoutDto>>>
{
    private readonly IMediator _mediator;

    public GetWorkoutsWithContraIndicationsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/workouts/with-contraindications");

    }

    public override async Task HandleAsync(GetWorkoutsWithContraIndicationsRequest req, CancellationToken ct)
    {
        var query = new GetWorkoutsForUserQuery(req.UserId, req.IncludeDeleted);
        var result = await _mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<List<WorkoutDto>>.Success(result, requestId);
    }
}
