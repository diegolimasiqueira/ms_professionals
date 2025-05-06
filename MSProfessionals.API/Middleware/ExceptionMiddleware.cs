using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MSProfessionals.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MSProfessionals.API.Middleware;

/// <summary>
/// Middleware to handle exceptions globally
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the ExceptionMiddleware
    /// </summary>
    /// <param name="next">Request delegate</param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            Message = GetErrorMessage(exception),
            Details = GetErrorDetails(exception),
            Errors = GetValidationErrors(exception)
        };

        context.Response.StatusCode = GetStatusCode(exception);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }

    private static string GetErrorMessage(Exception exception)
    {
        return exception switch
        {
            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx => 
                pgEx.SqlState switch
                {
                    "23505" => "Unique constraint violation",
                    "23502" => "Not null violation",
                    "23503" => "Foreign key violation",
                    _ => "An error occurred while saving the entity changes"
                },
            ValidationException valEx => valEx.Message,
            ProfessionalNotFoundException notFoundEx => notFoundEx.Message,
            UnauthorizedAccessException unauthorizedEx => unauthorizedEx.Message,
            JsonException jsonEx => "Invalid JSON format",
            _ => "Internal Server Error"
        };
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx =>
                pgEx.SqlState switch
                {
                    "23505" => (int)HttpStatusCode.Conflict,
                    "23502" => (int)HttpStatusCode.BadRequest,
                    "23503" => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                },
            ValidationException => (int)HttpStatusCode.BadRequest,
            ProfessionalNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            JsonException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    private static string? GetErrorDetails(Exception exception)
    {
        if (exception is DbUpdateException dbEx)
        {
            if (dbEx.InnerException is PostgresException pgEx)
            {
                return $"{pgEx.SqlState}: {pgEx.Message}\n\nPOSITION: {pgEx.Position}\n\nDETAIL: {pgEx.Detail}";
            }
            return dbEx.InnerException?.Message;
        }
        return exception.Message;
    }

    private static object? GetValidationErrors(Exception exception)
    {
        if (exception is JsonException)
        {
            return new
            {
                json = new[] { "Invalid JSON format" }
            };
        }
        return null;
    }
}