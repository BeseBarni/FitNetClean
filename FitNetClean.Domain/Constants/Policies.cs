namespace FitNetClean.Domain.Constants;

public static class Policies
{
    // Role-based policies
    public const string AdminOnly = "AdminOnly";
    public const string ProgramWriterOrAdmin = "ProgramWriterOrAdmin";
    public const string ProgramReaderOrHigher = "ProgramReaderOrHigher";
    
    // Custom policies
    public const string ActiveUsersOnly = "ActiveUsersOnly";
}
