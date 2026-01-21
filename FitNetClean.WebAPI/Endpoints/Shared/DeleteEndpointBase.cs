using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.Shared.Commands;
using FitNetClean.WebAPI.Extensions;
using MediatR;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public abstract class DeleteEndpointBase<TEntity>(IMediator mediator) 
    : EndpointWithoutRequest<ApiResponse<bool>>
    where TEntity : class
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id");
        var result = await mediator.Send(new DeleteCommand<TEntity>(id), ct);
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
