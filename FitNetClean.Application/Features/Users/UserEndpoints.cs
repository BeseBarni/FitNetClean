using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Application.Features.Users.Commands;
using FitNetClean.Application.Features.Users.Queries;
using MediatR;

namespace FitNetClean.Application.Features.Users;

public record UpdateUserAvoidedContraIndicationsRequest
{
    public List<long> ContraIndicationIds { get; init; } = new();
}

public class UpdateUserAvoidedContraIndicationsEndpoint : Endpoint<UpdateUserAvoidedContraIndicationsRequest, ApiResponse<bool>>
{
    private readonly IMediator _mediator;

    public UpdateUserAvoidedContraIndicationsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/users/{id}/avoided-contraindications");

    }

    public override async Task HandleAsync(UpdateUserAvoidedContraIndicationsRequest req, CancellationToken ct)
    {
        var userId = Route<long>("id");
        var command = new UpdateUserAvoidedContraIndicationsCommand(userId, req.ContraIndicationIds);
        var result = await _mediator.Send(command, ct);
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

public class GetUserAvoidedContraIndicationsEndpoint : Endpoint<IdRequest, ApiResponse<List<ContraIndicationDto>>>
{
    private readonly IMediator _mediator;

    public GetUserAvoidedContraIndicationsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users/{id}/avoided-contraindications");

    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetUserAvoidedContraIndicationsQuery(req.Id);
        var result = await _mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<List<ContraIndicationDto>>.Success(result, requestId);
    }
}
