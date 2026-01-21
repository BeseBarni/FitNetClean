using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.DTOs;
using MediatR;

namespace FitNetClean.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<Result<AuthResponse>>;

public class RegisterCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        return identityService.RegisterAsync(request.Request, ct);
    }
}
