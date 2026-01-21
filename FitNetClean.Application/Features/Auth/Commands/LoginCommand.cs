using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.DTOs;
using MediatR;

namespace FitNetClean.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<Result<AuthResponse>>;

public class LoginCommandHandler(IIdentityService identityService)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        return identityService.LoginAsync(request.Request, ct);
    }
}
