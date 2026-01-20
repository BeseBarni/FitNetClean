using FastEndpoints;
using FitNetClean.Application.Features.Shared.Commands;
using MediatR;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public abstract class DeleteEndpointBase<TEntity> : EndpointWithoutRequest
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

        if (!result)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        HttpContext.Response.StatusCode = 204; // No Content
    }
}
