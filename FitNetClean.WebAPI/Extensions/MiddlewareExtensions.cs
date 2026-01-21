using FitNetClean.WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;

namespace FitNetClean.WebAPI.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    public static IApplicationBuilder UseAuthorizedRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthorizedRequestLoggingMiddleware>();
    }
}
