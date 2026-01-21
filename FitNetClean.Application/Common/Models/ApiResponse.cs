namespace FitNetClean.Application.Common.Models;

public record ApiResponse<T>
{
    public int StatusCode { get; init; }
    public T? Content { get; init; }
    public Guid Identity { get; init; }

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
            Identity = requestId
        };
    }

    public static ApiResponse<T> Error(Guid requestId, int statusCode = 500, T? content = default)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Content = content,
            Identity = requestId
        };
    }
}
