namespace FitNetClean.WebAPI.Middleware;

public class RequestIdentityMiddleware
{
    private readonly RequestDelegate _next;
    private const string RequestIdKey = "RequestId";
    private const string RequestIdHeader = "X-Request-ID";

    public RequestIdentityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid();
        
        context.Items[RequestIdKey] = requestId;
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestIdHeader] = requestId.ToString();
            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class RequestIdentityMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestIdentity(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestIdentityMiddleware>();
    }
}
