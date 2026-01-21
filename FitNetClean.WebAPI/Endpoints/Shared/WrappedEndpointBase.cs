using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.WebAPI.Extensions;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public abstract class WrappedEndpointBase<TRequest, TResponse> : Endpoint<TRequest, ApiResponse<TResponse>>
{
    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        try
        {
            var result = await ExecuteAsync(req, ct);
            var requestId = HttpContext.GetRequestId();

            Response = ApiResponse<TResponse>.Success(result, requestId, 200);
        }
        catch (Exception)
        {
            var requestId = HttpContext.GetRequestId();
            Response = ApiResponse<TResponse>.Error(requestId, 500);
            HttpContext.Response.StatusCode = 500;
            throw;
        }
    }

    protected abstract Task<TResponse> ExecuteAsync(TRequest req, CancellationToken ct);

    protected void SendNotFound()
    {
        var requestId = HttpContext.GetRequestId();
        Response = ApiResponse<TResponse>.NotFound(requestId);
        HttpContext.Response.StatusCode = 404;
    }
}

public abstract class WrappedEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<ApiResponse<TResponse>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var result = await ExecuteAsync(ct);
            var requestId = HttpContext.GetRequestId();

            Response = ApiResponse<TResponse>.Success(result, requestId, 200);
        }
        catch (Exception)
        {
            var requestId = HttpContext.GetRequestId();
            Response = ApiResponse<TResponse>.Error(requestId, 500);
            HttpContext.Response.StatusCode = 500;
            throw;
        }
    }

    protected abstract Task<TResponse> ExecuteAsync(CancellationToken ct);

    protected void SendNotFound()
    {
        var requestId = HttpContext.GetRequestId();
        Response = ApiResponse<TResponse>.NotFound(requestId);
        HttpContext.Response.StatusCode = 404;
    }
}


