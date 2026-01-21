using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.WorkoutGroups;
using FitNetClean.Application.Features.WorkoutGroups.Commands;
using FitNetClean.Domain.Entities;
using FitNetClean.WebAPI.Endpoints.Shared;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.WorkoutGroups;

public class GetWorkoutGroupsEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<WorkoutGroup, WorkoutGroupDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workout-groups");

    }
}

public class GetWorkoutGroupByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<WorkoutGroup, WorkoutGroupDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/workout-groups/{id}");

    }
}

public class CreateWorkoutGroupEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<WorkoutGroup, WorkoutGroupDto, CreateWorkoutGroupRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/workout-groups");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class UpdateWorkoutGroupEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<WorkoutGroup, WorkoutGroupDto, UpdateWorkoutGroupRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/workout-groups/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class DeleteWorkoutGroupEndpoint(IMediator mediator)
    : DeleteEndpointBase<WorkoutGroup>(mediator)
{
    public override void Configure()
    {
        Delete("/workout-groups/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class AddExerciseToWorkoutGroupEndpoint(IMediator mediator) 
    : Endpoint<AddExerciseRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/workout-groups/{id}/exercises");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }

    public override async Task HandleAsync(AddExerciseRequest req, CancellationToken ct)
    {
        var workoutGroupId = Route<long>("id");
        var command = new AddExerciseToWorkoutGroupCommand(workoutGroupId, req.ExerciseId);
        var result = await mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        if (!result)
        {
            Response = ApiResponse<bool>.NotFound(requestId, false);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<bool>.Success(result, requestId);
    }
}

public class RemoveExerciseFromWorkoutGroupEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<ApiResponse<bool>>
{
    public override void Configure()
    {
        Delete("/workout-groups/{id}/exercises/{exerciseId}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var workoutGroupId = Route<long>("id");
        var exerciseId = Route<long>("exerciseId");
        var command = new RemoveExerciseFromWorkoutGroupCommand(workoutGroupId, exerciseId);
        var result = await mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        if (!result)
        {
            Response = ApiResponse<bool>.NotFound(requestId, false);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<bool>.Success(result, requestId);
    }
}
