using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Application.Features.Workouts.Commands;
using FitNetClean.Application.Features.Workouts.Queries;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Workouts;

public class GetWorkoutsEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Workout, WorkoutDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workouts");
        AllowAnonymous();
    }
}

public class GetWorkoutByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Workout, WorkoutDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workouts/{id}");
        AllowAnonymous();
    }
}

public class GetWorkoutDetailEndpoint(IMediator mediator) : Endpoint<IdRequest, WorkoutDetailDto?>
{
    public override void Configure()
    {
        Get("/workouts/{id}/details");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetWorkoutDetailQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public record CreateWorkoutRequest(
    string CodeName, 
    string Title, 
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes);

public class CreateWorkoutEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Workout, WorkoutDto, CreateWorkoutRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/workouts");
        AllowAnonymous();
    }
}

public record UpdateWorkoutRequest(
    string CodeName, 
    string Title, 
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes);

public class UpdateWorkoutEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Workout, WorkoutDto, UpdateWorkoutRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/workouts/{id}");
        AllowAnonymous();
    }
}

public class DeleteWorkoutEndpoint(IMediator mediator)
    : DeleteEndpointBase<Workout>(mediator)
{
    public override void Configure()
    {
        Delete("/workouts/{id}");
        AllowAnonymous();
    }
}

public record AddWorkoutGroupRequest
{
    public long WorkoutGroupId { get; init; }
}

public class AddWorkoutGroupToWorkoutEndpoint(IMediator mediator) 
    : Endpoint<AddWorkoutGroupRequest, bool>
{
    public override void Configure()
    {
        Post("/workouts/{id}/workout-groups");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddWorkoutGroupRequest req, CancellationToken ct)
    {
        var workoutId = Route<long>("id");
        var command = new AddWorkoutGroupToWorkoutCommand(workoutId, req.WorkoutGroupId);
        var result = await mediator.Send(command, ct);

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}
