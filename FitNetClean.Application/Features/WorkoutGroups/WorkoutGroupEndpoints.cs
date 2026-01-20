using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Application.Features.WorkoutGroups.Commands;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.WorkoutGroups;

public class GetWorkoutGroupsEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<WorkoutGroup, WorkoutGroupDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workout-groups");
        AllowAnonymous();
    }
}

public class GetWorkoutGroupByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<WorkoutGroup, WorkoutGroupDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workout-groups/{id}");
        AllowAnonymous();
    }
}

public record CreateWorkoutGroupRequest(long WorkoutId, string Title);

public class CreateWorkoutGroupEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<WorkoutGroup, WorkoutGroupDto, CreateWorkoutGroupRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/workout-groups");
        AllowAnonymous();
    }
}

public record UpdateWorkoutGroupRequest(long WorkoutId, string Title);

public class UpdateWorkoutGroupEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<WorkoutGroup, WorkoutGroupDto, UpdateWorkoutGroupRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/workout-groups/{id}");
        AllowAnonymous();
    }
}

public class DeleteWorkoutGroupEndpoint(IMediator mediator)
    : DeleteEndpointBase<WorkoutGroup>(mediator)
{
    public override void Configure()
    {
        Delete("/workout-groups/{id}");
        AllowAnonymous();
    }
}

public record AddExerciseRequest
{
    public long ExerciseId { get; init; }
}

public class AddExerciseToWorkoutGroupEndpoint(IMediator mediator) 
    : Endpoint<AddExerciseRequest, bool>
{
    public override void Configure()
    {
        Post("/workout-groups/{id}/exercises");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddExerciseRequest req, CancellationToken ct)
    {
        var workoutGroupId = Route<long>("id");
        var command = new AddExerciseToWorkoutGroupCommand(workoutGroupId, req.ExerciseId);
        var result = await mediator.Send(command, ct);

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public class RemoveExerciseFromWorkoutGroupEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Delete("/workout-groups/{id}/exercises/{exerciseId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var workoutGroupId = Route<long>("id");
        var exerciseId = Route<long>("exerciseId");
        var command = new RemoveExerciseFromWorkoutGroupCommand(workoutGroupId, exerciseId);
        var result = await mediator.Send(command, ct);

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}
