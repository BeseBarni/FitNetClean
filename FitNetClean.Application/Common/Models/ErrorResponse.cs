namespace FitNetClean.Application.Common.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public ErrorResponse(string message, int statusCode, Dictionary<string, string[]> errors)
    {
        Message = message;
        StatusCode = statusCode;
        Errors = errors;
    }
}
