namespace FitNetClean.Application.Common.Models;

public record ApiResponse<T>
{
    public int StatusCode { get; init; }
    public T? Content { get; init; }
    public Guid Identity { get; init; }
    public string? ErrorMessage { get; init; }

    public static ApiResponse<T> Success(T content, Guid requestId, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Content = content,
            Identity = requestId
        };
    }

    public static ApiResponse<T> NotFound(Guid requestId, T? content = default)
    {
        return new ApiResponse<T>
        {
            StatusCode = 404,
            Content = content,
            Identity = requestId,
            ErrorMessage = "Resource not found"
        };
    }

    public static ApiResponse<T> Error(Guid requestId, int statusCode = 500, string? errorMessage = null)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Content = default,
            Identity = requestId,
            ErrorMessage = errorMessage ?? "An error occurred"
        };
    }
}
