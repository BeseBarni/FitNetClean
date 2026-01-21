using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.Auth.Commands;
using MediatR;

namespace FitNetClean.Application.Features.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, ApiResponse<AuthResponse>>
{
    private readonly IMediator _mediator;

    public RegisterEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var command = new RegisterCommand(req);
        var authResponse = await _mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<AuthResponse>.Success(authResponse, requestId, 201);
        HttpContext.Response.StatusCode = 201;
    }
}

public class LoginEndpoint : Endpoint<LoginRequest, ApiResponse<AuthResponse>>
{
    private readonly IMediator _mediator;

    public LoginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var command = new LoginCommand(req);
        var authResponse = await _mediator.Send(command, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<AuthResponse>.Success(authResponse, requestId);
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
