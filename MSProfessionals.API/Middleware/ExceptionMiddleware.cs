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
            StatusCode = GetStatusCode(exception),
            Details = GetErrorDetails(exception),
            Errors = GetValidationErrors(exception),
            StackTrace = exception.StackTrace
        };

        context.Response.StatusCode = response.StatusCode;

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
            ValidationException valEx => "Validation failed",
            ProfessionalServiceNotFoundException notFoundEx => notFoundEx.Message,
            JsonException jsonEx => "Invalid JSON format",
            _ => exception.Message
        };
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx =>
                pgEx.SqlState switch
                {
                    "23505" => 409, // Conflict
                    "23502" => 400, // Bad Request
                    "23503" => 400, // Bad Request
                    _ => 500
                },
            ValidationException => 400,
            ProfessionalServiceNotFoundException => 404,
            JsonException => 400,
            _ => 500
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
        return exception.InnerException?.Message;
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