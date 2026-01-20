using FitNetClean.Application.Common.Exceptions;
using FitNetClean.Application.Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace FitNetClean.WebAPI.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            ValidationException validationException => new ErrorResponse(
                "One or more validation errors occurred.",
                (int)HttpStatusCode.BadRequest,
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    ))
            {
                TraceId = context.TraceIdentifier
            },

            DeleteValidationException deleteValidationException => new ErrorResponse(
                deleteValidationException.Message,
                (int)HttpStatusCode.BadRequest)
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "Dependencies", deleteValidationException.Dependencies.ToArray() }
                },
                TraceId = context.TraceIdentifier
            },

            KeyNotFoundException => new ErrorResponse(
                "The requested resource was not found.",
                (int)HttpStatusCode.NotFound)
            {
                TraceId = context.TraceIdentifier
            },

            UnauthorizedAccessException => new ErrorResponse(
                "Unauthorized access.",
                (int)HttpStatusCode.Unauthorized)
            {
                TraceId = context.TraceIdentifier
            },

            _ => new ErrorResponse(
                "An error occurred while processing your request.",
                (int)HttpStatusCode.InternalServerError)
            {
                TraceId = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = errorResponse.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(errorResponse, options));
    }
}
