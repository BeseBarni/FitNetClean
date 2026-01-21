using System.Security.Claims;
using System.Text;

namespace FitNetClean.WebAPI.Middleware;

public class AuthorizedRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizedRequestLoggingMiddleware> _logger;

    public AuthorizedRequestLoggingMiddleware(RequestDelegate next, ILogger<AuthorizedRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if user is authenticated (requires authorization)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            await LogAuthorizedRequestAsync(context);
        }

        await _next(context);
    }

    private async Task LogAuthorizedRequestAsync(HttpContext context)
    {
        try
        {
            // a. HTTP metódus
            var httpMethod = context.Request.Method;

            // b. Végpont neve
            var endpoint = context.Request.Path.ToString();

            // c. Request body (JSON vagy XML)
            var requestBody = await ReadRequestBodyAsync(context);

            // d. User azonosító
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";

            // e. User név
            var userName = context.User.FindFirst(ClaimTypes.Name)?.Value ?? 
                          context.User.FindFirst(ClaimTypes.Email)?.Value ?? 
                          "Unknown";

            // Structured logging
            _logger.LogInformation(
                "Authorized Request | Method: {HttpMethod} | Endpoint: {Endpoint} | UserId: {UserId} | UserName: {UserName} | Body: {RequestBody}",
                httpMethod,
                endpoint,
                userId,
                userName,
                requestBody
            );

            // Console kimenet (színes)
            LogToConsole(httpMethod, endpoint, userId, userName, requestBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging authorized request");
        }
    }

    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        var body = context.Request.Body;
        var buffer = new byte[Convert.ToInt32(context.Request.ContentLength ?? 0)];

        if (buffer.Length == 0)
            return "[Empty]";

        await body.ReadAsync(buffer, 0, buffer.Length);
        body.Seek(0, SeekOrigin.Begin);

        var bodyAsText = Encoding.UTF8.GetString(buffer);

        // Truncate if too long
        if (bodyAsText.Length > 1000)
            bodyAsText = bodyAsText.Substring(0, 1000) + "... [truncated]";

        return string.IsNullOrWhiteSpace(bodyAsText) ? "[Empty]" : bodyAsText;
    }

    private void LogToConsole(string httpMethod, string endpoint, string userId, string userName, string requestBody)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=== AUTHORIZED REQUEST LOG ===");
        Console.ResetColor();

        Console.Write("Method: ");
        Console.ForegroundColor = GetMethodColor(httpMethod);
        Console.WriteLine(httpMethod);
        Console.ResetColor();

        Console.Write("Endpoint: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(endpoint);
        Console.ResetColor();

        Console.Write("User ID: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(userId);
        Console.ResetColor();

        Console.Write("User Name: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(userName);
        Console.ResetColor();

        Console.Write("Request Body: ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(requestBody);
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==============================\n");
        Console.ResetColor();
    }

    private ConsoleColor GetMethodColor(string method)
    {
        return method switch
        {
            "GET" => ConsoleColor.Blue,
            "POST" => ConsoleColor.Green,
            "PUT" => ConsoleColor.Yellow,
            "DELETE" => ConsoleColor.Red,
            "PATCH" => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };
    }
}
