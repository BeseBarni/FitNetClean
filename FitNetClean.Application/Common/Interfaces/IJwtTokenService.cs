using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}
