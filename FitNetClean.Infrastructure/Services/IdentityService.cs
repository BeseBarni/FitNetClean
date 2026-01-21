using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Constants;
using FitNetClean.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitNetClean.Infrastructure.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenService jwtTokenService) : IIdentityService
{
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<AuthResponse>.Failure("Invalid email or password");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Result<AuthResponse>.Failure("Invalid email or password");
        }

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResponse(
            token,
            user.Email!,
            user.FullName,
            user.City,
            user.Country,
            user.ProfilePictureUrl
        );
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            City = request.City,
            Country = request.Country,
            ProfilePictureUrl = request.ProfilePictureUrl,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<AuthResponse>.Failure($"Registration failed: {errors}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.ProgramReader);
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return Result<AuthResponse>.Failure($"Failed to assign role: {errors}");
        }

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResponse(
            token,
            user.Email!,
            user.FullName,
            user.City,
            user.Country,
            user.ProfilePictureUrl
        );
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(long userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result<UserDto>.Failure("User not found");
        }

        return new UserDto(
            user.Id,
            user.Email!,
            user.FullName,
            user.City,
            user.Country,
            user.ProfilePictureUrl
        );
    }
}
