using FitNetClean.Domain.Common;

namespace FitNetClean.Infrastructure.Settings;

public class DatabaseSettings : ISettings
{
    public static string SectionName => "Database";
    public string ConnectionString { get; set; } = string.Empty;
}
