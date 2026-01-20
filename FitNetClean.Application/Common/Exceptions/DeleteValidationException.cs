namespace FitNetClean.Application.Common.Exceptions;

public class DeleteValidationException : Exception
{
    public List<string> Dependencies { get; }

    public DeleteValidationException(string entityType, List<string> dependencies)
        : base($"Cannot delete {entityType} because it has active dependencies: {string.Join(", ", dependencies)}")
    {
        Dependencies = dependencies;
    }
}
