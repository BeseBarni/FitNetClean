namespace FitNetClean.Application.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string City,
    string Country,
    string? ProfilePictureUrl
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string Token,
    string Email,
    string FullName,
    string City,
    string Country,
    string? ProfilePictureUrl
);

public record UserDto(
    long Id,
    string Email,
    string FullName,
    string City,
    string Country,
    string? ProfilePictureUrl
);
