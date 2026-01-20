using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FitNetClean.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<AuthResponse>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        var req = request.Request;

        var user = new ApplicationUser
        {
            UserName = req.Email,
            Email = req.Email,
            FullName = req.FullName,
            City = req.City,
            Country = req.Country,
            ProfilePictureUrl = req.ProfilePictureUrl,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, req.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponse(
            token,
            user.Email!,
            user.FullName,
            user.City,
            user.Country,
            user.ProfilePictureUrl
        );
    }
}
