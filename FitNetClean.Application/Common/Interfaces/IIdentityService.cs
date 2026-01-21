using FitNetClean.Application.DTOs;

namespace FitNetClean.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<UserDto>> GetUserByIdAsync(long userId, CancellationToken ct = default);
}
