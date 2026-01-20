using AutoMapper;
using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Exercises.Commands;
using FitNetClean.Application.Features.Exercises.Queries;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Exercises;

public class GetExercisesEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Exercise, ExerciseDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/exercises");
        AllowAnonymous();
    }
}

public class GetExerciseByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Exercise, ExerciseDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/exercises/{id}");
        AllowAnonymous();
    }
}

public class GetExerciseWorkoutProgramsEndpoint(IMediator mediator) : Endpoint<IdRequest, ExerciseWorkoutProgramsDto?>
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

        if (result == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public record CreateExerciseRequest(
    string Name, 
    Measurement Repetition, 
    long WorkoutId, 
    long? WorkoutGroupId, 
    long? EquipmentId);

public class CreateExerciseEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Exercise, ExerciseDto, CreateExerciseRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/exercises");
        AllowAnonymous();
    }
}

public record UpdateExerciseRequest(
    string Name, 
    Measurement Repetition, 
    long WorkoutId, 
    long? WorkoutGroupId, 
    long? EquipmentId);

public class UpdateExerciseEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Exercise, ExerciseDto, UpdateExerciseRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/exercises/{id}");
        AllowAnonymous();
    }
}

public class DeleteExerciseEndpoint(IMediator mediator)
    : DeleteEndpointBase<Exercise>(mediator)
{
    public override void Configure()
    {
        Delete("/exercises/{id}");
        AllowAnonymous();
    }
}

public record AddContraIndicationRequest
{
    public long ContraIndicationId { get; init; }
}

public class AddContraIndicationToExerciseEndpoint(IMediator mediator) 
    : Endpoint<AddContraIndicationRequest, bool>
{
    public override void Configure()
    {
        Post("/exercises/{id}/contraindications");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddContraIndicationRequest req, CancellationToken ct)
    {
        var exerciseId = Route<long>("id");
        var command = new AddContraIndicationToExerciseCommand(exerciseId, req.ContraIndicationId);
        var result = await mediator.Send(command, ct);

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public class RemoveContraIndicationFromExerciseEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Delete("/exercises/{id}/contraindications/{contraIndicationId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var exerciseId = Route<long>("id");
        var contraIndicationId = Route<long>("contraIndicationId");
        var command = new RemoveContraIndicationFromExerciseCommand(exerciseId, contraIndicationId);
        var result = await mediator.Send(command, ct);

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}
