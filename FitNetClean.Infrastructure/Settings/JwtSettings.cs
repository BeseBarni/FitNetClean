using FitNetClean.Domain.Common;

namespace FitNetClean.Infrastructure.Settings;

public record JwtSettings : ISettings
{
    public static string SectionName => "Jwt";
    public string Secret { get; init; } = string.Empty;
}
