namespace FitNetClean.Application.Common.Validation;

public interface IDeleteValidator
{
    Task<(bool CanDelete, List<string> Dependencies)> ValidateDeleteAsync<T>(long id, CancellationToken ct) where T : class;
}
