using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Users;
using FitNetClean.Application.Features.Users.Commands;
using FitNetClean.Application.Features.Users.Queries;
using FitNetClean.WebAPI.Endpoints.Shared;
using FitNetClean.WebAPI.Extensions;
using MediatR;

namespace FitNetClean.WebAPI.Endpoints.Users;

public class UpdateUserAvoidedContraIndicationsEndpoint(IMediator mediator) 
    : Endpoint<UpdateUserAvoidedContraIndicationsRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Put("/users/{id}/avoided-contraindications");
    }

    public override async Task HandleAsync(UpdateUserAvoidedContraIndicationsRequest req, CancellationToken ct)
    {
        var userId = Route<long>("id");
        var command = new UpdateUserAvoidedContraIndicationsCommand(userId, req.ContraIndicationIds);
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

public class GetUserAvoidedContraIndicationsEndpoint(IMediator mediator) 
    : Endpoint<IdRequest, ApiResponse<List<ContraIndicationDto>>>
{
    public override void Configure()
    {
        Get("/users/{id}/avoided-contraindications");
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetUserAvoidedContraIndicationsQuery(req.Id);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<List<ContraIndicationDto>>.Success(result, requestId);
    }
}
