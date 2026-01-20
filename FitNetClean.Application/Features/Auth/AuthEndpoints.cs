using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Auth.Commands;
using MediatR;

namespace FitNetClean.Application.Features.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, AuthResponse>
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
        var response = await _mediator.Send(command, ct);

        HttpContext.Response.StatusCode = 201;
        Response = response;
    }
}

public class LoginEndpoint : Endpoint<LoginRequest, AuthResponse>
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
        var response = await _mediator.Send(command, ct);

        Response = response;
    }
}

public class LogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/auth/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // For JWT, logout is handled client-side by removing the token
        // Server-side logout would require token blacklisting (not implemented here)
        
        HttpContext.Response.StatusCode = 204; // No Content
        await Task.CompletedTask;
    }
}
