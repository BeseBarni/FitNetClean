using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Extensions;
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
        var requestId = GetRequestId();

        if (result == null)
        {
            Response = ApiResponse<WorkoutDetailDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<WorkoutDetailDto?>.Success(result, requestId);
    }

    private Guid GetRequestId()
    {
        return HttpContext.Items.TryGetValue("RequestId", out var value) && value is Guid guid
            ? guid
            : Guid.Empty;
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
        Policies(FitNetClean.Domain.Constants.Policies.ProgramWriterOrAdmin);
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

public record AddWorkoutGroupRequest
{
    public long WorkoutGroupId { get; init; }
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

public record FavoriteWorkoutRequest
{
    public long UserId { get; init; }
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
        var requestId = GetRequestId();

        if (!result)
        {
            Response = ApiResponse<bool>.NotFound(requestId, false);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<bool>.Success(result, requestId);
    }

    private Guid GetRequestId()
    {
        return HttpContext.Items.TryGetValue("RequestId", out var value) && value is Guid guid
            ? guid
            : Guid.Empty;
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
        var requestId = GetRequestId();

        if (!result)
        {
            Response = ApiResponse<bool>.NotFound(requestId, false);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<bool>.Success(result, requestId);
    }

    private Guid GetRequestId()
    {
        return HttpContext.Items.TryGetValue("RequestId", out var value) && value is Guid guid
            ? guid
            : Guid.Empty;
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
        var requestId = GetRequestId();

        Response = ApiResponse<List<WorkoutDto>>.Success(result, requestId);
    }

    private Guid GetRequestId()
    {
        return HttpContext.Items.TryGetValue("RequestId", out var value) && value is Guid guid
            ? guid
            : Guid.Empty;
    }
}

