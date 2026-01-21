using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Auth.Commands;
using FitNetClean.WebAPI.Extensions;
using MediatR;

namespace FitNetClean.WebAPI.Endpoints.Auth;

public class RegisterEndpoint(IMediator mediator) : Endpoint<RegisterRequest, ApiResponse<AuthResponse>>
{
    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var command = new RegisterCommand(req);
        var result = await mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        if (result.IsFailure)
        {
            Response = ApiResponse<AuthResponse>.Error(requestId, 400, result.Error);
            HttpContext.Response.StatusCode = 400;
            return;
        }

        Response = ApiResponse<AuthResponse>.Success(result.Value, requestId, 201);
        HttpContext.Response.StatusCode = 201;
    }
}

public class LoginEndpoint(IMediator mediator) : Endpoint<LoginRequest, ApiResponse<AuthResponse>>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var command = new LoginCommand(req);
        var result = await mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        if (result.IsFailure)
        {
            Response = ApiResponse<AuthResponse>.Error(requestId, 401, result.Error);
            HttpContext.Response.StatusCode = 401;
            return;
        }

        Response = ApiResponse<AuthResponse>.Success(result.Value, requestId);
    }
}

public class LogoutEndpoint : EndpointWithoutRequest<ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/auth/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var requestId = HttpContext.GetRequestId();
        
        Response = ApiResponse<bool>.Success(true, requestId, 204);
        HttpContext.Response.StatusCode = 204;
        await Task.CompletedTask;
    }
}
