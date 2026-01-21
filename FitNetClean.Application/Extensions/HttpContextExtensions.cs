using Microsoft.AspNetCore.Http;

namespace FitNetClean.Application.Extensions;

public static class HttpContextExtensions
{
    private const string RequestIdKey = "RequestId";

    public static Guid GetRequestId(this HttpContext context)
    {
        return context.Items.TryGetValue(RequestIdKey, out var value) && value is Guid guid
            ? guid
            : Guid.Empty;
    }
}
