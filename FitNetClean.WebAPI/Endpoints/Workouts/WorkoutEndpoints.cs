using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Workouts;
using FitNetClean.Application.Features.Workouts.Commands;
using FitNetClean.Application.Features.Workouts.Queries;
using FitNetClean.Domain.Entities;
using FitNetClean.WebAPI.Endpoints.Shared;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Workouts;

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

    }
}

public class GetWorkoutDetailEndpoint(IMediator mediator) : Endpoint<IdRequest, ApiResponse<WorkoutDetailDto?>>
{
    public override void Configure()
    {
        Get("/workouts/{id}/details");

    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetWorkoutDetailQuery(req.Id);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        if (result == null)
        {
            Response = ApiResponse<WorkoutDetailDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<WorkoutDetailDto?>.Success(result, requestId);
    }
}

public class CreateWorkoutEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Workout, WorkoutDto, CreateWorkoutRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/workouts");
        Policies(FitNetClean.Domain.Constants.Policies.ProgramWriterOrAdmin);
    }
}

public class UpdateWorkoutEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Workout, WorkoutDto, UpdateWorkoutRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/workouts/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.ProgramWriterOrAdmin);
    }
}

public class DeleteWorkoutEndpoint(IMediator mediator)
    : DeleteEndpointBase<Workout>(mediator)
{
    public override void Configure()
    {
        Delete("/workouts/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class AddWorkoutGroupToWorkoutEndpoint(IMediator mediator) 
    : Endpoint<AddWorkoutGroupRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/workouts/{id}/workout-groups");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }

    public override async Task HandleAsync(AddWorkoutGroupRequest req, CancellationToken ct)
    {
        var workoutId = Route<long>("id");
        var command = new AddWorkoutGroupToWorkoutCommand(workoutId, req.WorkoutGroupId);
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

public class AddWorkoutToFavoritesEndpoint(IMediator mediator) 
    : Endpoint<FavoriteWorkoutRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/workouts/{id}/favorites");

    }

    public override async Task HandleAsync(FavoriteWorkoutRequest req, CancellationToken ct)
    {
        var workoutId = Route<long>("id");
        var command = new AddWorkoutToFavoritesCommand(req.UserId, workoutId);
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

public class RemoveWorkoutFromFavoritesEndpoint(IMediator mediator) 
    : Endpoint<FavoriteWorkoutRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Delete("/workouts/{id}/favorites");

    }

    public override async Task HandleAsync(FavoriteWorkoutRequest req, CancellationToken ct)
    {
        var workoutId = Route<long>("id");
        var command = new RemoveWorkoutFromFavoritesCommand(req.UserId, workoutId);
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

public class GetUserFavoriteWorkoutsEndpoint(IMediator mediator) 
    : Endpoint<IdRequest, ApiResponse<List<WorkoutDto>>>
{
    public override void Configure()
    {
        Get("/users/{id}/favorite-workouts");

    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetUserFavoriteWorkoutsQuery(req.Id);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<List<WorkoutDto>>.Success(result, requestId);
    }
}

