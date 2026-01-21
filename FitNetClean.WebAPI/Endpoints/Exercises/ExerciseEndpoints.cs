using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Exercises;
using FitNetClean.Application.Features.Exercises.Commands;
using FitNetClean.Application.Features.Exercises.Queries;
using FitNetClean.Domain.Entities;
using FitNetClean.WebAPI.Endpoints.Shared;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Exercises;

public class GetExercisesEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Exercise, ExerciseDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/exercises");

    }
}

public class GetExerciseByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Exercise, ExerciseDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/exercises/{id}");

    }
}

public class GetExerciseWorkoutProgramsEndpoint(IMediator mediator) : Endpoint<IdRequest, ApiResponse<ExerciseWorkoutProgramsDto?>>
{
    public override void Configure()
    {
        Get("/exercises/{id}/workout-programs");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetExerciseWorkoutProgramsQuery(req.Id);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        if (result == null)
        {
            Response = ApiResponse<ExerciseWorkoutProgramsDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<ExerciseWorkoutProgramsDto?>.Success(result, requestId);
    }
}

public class CreateExerciseEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Exercise, ExerciseDto, CreateExerciseRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/exercises");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class UpdateExerciseEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Exercise, ExerciseDto, UpdateExerciseRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/exercises/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class DeleteExerciseEndpoint(IMediator mediator)
    : DeleteEndpointBase<Exercise>(mediator)
{
    public override void Configure()
    {
        Delete("/exercises/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class AddContraIndicationToExerciseEndpoint(IMediator mediator) 
    : Endpoint<AddContraIndicationRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/exercises/{id}/contraindications");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }

    public override async Task HandleAsync(AddContraIndicationRequest req, CancellationToken ct)
    {
        var exerciseId = Route<long>("id");
        var command = new AddContraIndicationToExerciseCommand(exerciseId, req.ContraIndicationId);
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

public class RemoveContraIndicationFromExerciseEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<ApiResponse<bool>>
{
    public override void Configure()
    {
        Delete("/exercises/{id}/contraindications/{contraIndicationId}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var exerciseId = Route<long>("id");
        var contraIndicationId = Route<long>("contraIndicationId");
        var command = new RemoveContraIndicationFromExerciseCommand(exerciseId, contraIndicationId);
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

