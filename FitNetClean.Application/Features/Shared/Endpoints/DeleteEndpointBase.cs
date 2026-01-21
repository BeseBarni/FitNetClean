using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.Shared.Commands;
using MediatR;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public abstract class DeleteEndpointBase<TEntity> : EndpointWithoutRequest<ApiResponse<bool>>
    where TEntity : class
{
    private readonly IMediator _mediator;

    protected DeleteEndpointBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id");
        var result = await _mediator.Send(new DeleteCommand<TEntity>(id), ct);
        var requestId = HttpContext.GetRequestId();

        if (!result)
        {
            Response = ApiResponse<bool>.NotFound(requestId, false);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<bool>.Success(true, requestId, 204);
        HttpContext.Response.StatusCode = 204;
    }
}
