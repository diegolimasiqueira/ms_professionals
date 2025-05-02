using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Application.Common.Exceptions;

namespace MSProfessionals.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the ExceptionMiddleware
        /// </summary>
        /// <param name="next">Next middleware in the pipeline</param>
        /// <param name="logger">Logger</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = exception switch
            {
                DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx => 
                    HandlePostgresException(pgEx),
                NotFoundException notFoundEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Resource not found", notFoundEx.Message),
                ProfessionalNotFoundException profEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Professional not found", profEx.Message),
                ProfessionalServiceNotFoundException svcEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Professional service not found", svcEx.Message),
                ProfessionalProfessionNotFoundException profEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Professional profession not found", profEx.Message),
                ServiceNotFoundException svcEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Service not found", svcEx.Message),
                ProfessionalAddressNotFoundException addrEx => 
                    new ErrorResponse((int)HttpStatusCode.NotFound, "Professional address not found", addrEx.Message),
                ValidationException valEx => 
                    new ErrorResponse((int)HttpStatusCode.BadRequest, "Validation error", valEx.Message),
                ArgumentException argEx => 
                    new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid argument", argEx.Message),
                InvalidOperationException invOpEx => 
                    new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid operation", invOpEx.Message),
                UniqueConstraintViolationException uniqueEx => 
                    new ErrorResponse((int)HttpStatusCode.Conflict, "Unique constraint violation", uniqueEx.Message),
                _ => new ErrorResponse(StatusCodes.Status500InternalServerError, 
                    "An internal server error occurred", 
                    "An unexpected error occurred. Please try again later.")
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static ErrorResponse HandlePostgresException(PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                // Integrity constraint violations
                "23503" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Foreign key violation", 
                    GetForeignKeyErrorMessage(pgEx)),
                "23505" => new ErrorResponse(StatusCodes.Status409Conflict, 
                    "Unique constraint violation", 
                    GetUniqueConstraintMessage(pgEx)),
                "23514" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Check constraint violation", 
                    GetCheckViolationMessage(pgEx)),
                
                // Invalid data
                "22000" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Data error", 
                    "The provided data is invalid"),
                "22003" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Numeric value out of range", 
                    "A numeric value is out of range"),
                "22007" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Invalid datetime format", 
                    "The datetime format is invalid"),
                "22P02" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Invalid text representation", 
                    "The provided value has an invalid format"),

                // Invalid schema/object references
                "42703" => new ErrorResponse(StatusCodes.Status400BadRequest, 
                    "Column does not exist", 
                    GetColumnErrorMessage(pgEx)),
                "42P01" => new ErrorResponse(StatusCodes.Status500InternalServerError, 
                    "Table does not exist", 
                    "The requested table does not exist in the database"),
                
                // Transaction errors
                "40001" => new ErrorResponse(StatusCodes.Status409Conflict, 
                    "Serialization failure", 
                    "The transaction failed due to concurrent updates. Please try again."),
                "40P01" => new ErrorResponse(StatusCodes.Status409Conflict, 
                    "Deadlock detected", 
                    "The operation was cancelled due to a deadlock. Please try again."),

                // Default case
                _ => new ErrorResponse(StatusCodes.Status500InternalServerError, 
                    "Database error", 
                    $"A database error occurred: {pgEx.MessageText}")
            };
        }

        private static string GetForeignKeyErrorMessage(PostgresException exception)
        {
            var constraintName = exception.ConstraintName?.ToLower() ?? string.Empty;

            if (constraintName.Contains("phone_country_code_id"))
                return "The provided country code ID does not exist";
            if (constraintName.Contains("currency_id"))
                return "The provided currency ID does not exist";
            if (constraintName.Contains("preferred_language_id"))
                return "The provided language ID does not exist";
            if (constraintName.Contains("timezone_id"))
                return "The provided timezone ID does not exist";
            if (constraintName.Contains("country_id"))
                return "The provided country ID does not exist";
            if (constraintName.Contains("professional_id"))
                return "The provided professional ID does not exist";
            if (constraintName.Contains("profession_id"))
                return "The provided profession ID does not exist";
            if (constraintName.Contains("service_id"))
                return "The provided service ID does not exist";

            return $"The referenced {GetConstraintEntityName(constraintName)} does not exist";
        }

        private static string GetConstraintEntityName(string constraintName)
        {
            var parts = constraintName.Split('_');
            return parts.Length > 1 ? parts[^2] : "record";
        }

        private static string GetUniqueConstraintMessage(PostgresException exception)
        {
            var fieldName = GetUniqueConstraintFieldName(exception);
            var fieldValue = GetUniqueConstraintFieldValue(exception);
            return $"A {fieldName} with value '{fieldValue}' already exists";
        }

        private static string GetColumnErrorMessage(PostgresException exception)
        {
            var columnName = GetColumnNameFromMessage(exception.Message);
            return $"The column '{columnName}' does not exist in the table";
        }

        private static string GetCheckViolationMessage(PostgresException exception)
        {
            var constraintName = exception.ConstraintName?.ToLower() ?? string.Empty;

            if (constraintName.Contains("email_format"))
                return "The email address format is invalid";
            if (constraintName.Contains("phone_format"))
                return "The phone number format is invalid";
            if (constraintName.Contains("document_format"))
                return "The document ID format is invalid";
            if (constraintName.Contains("positive"))
                return "The value must be positive";
            if (constraintName.Contains("range"))
                return "The value is outside the allowed range";
            if (constraintName.Contains("length"))
                return "The text length is invalid";

            return "The provided data does not meet the validation rules";
        }

        private static string GetColumnNameFromMessage(string message)
        {
            var startIndex = message.IndexOf('"', message.IndexOf("column")) + 1;
            var endIndex = message.IndexOf('"', startIndex);

            if (startIndex > 0 && endIndex > startIndex)
            {
                return message.Substring(startIndex, endIndex - startIndex);
            }

            return "Unknown";
        }

        private static string GetUniqueConstraintFieldName(PostgresException exception)
        {
            var constraintName = exception.ConstraintName?.ToLower() ?? string.Empty;

            if (constraintName.Contains("email")) return "Email";
            if (constraintName.Contains("document_id")) return "Document ID";
            if (constraintName.Contains("phone_number")) return "Phone Number";
            if (constraintName.Contains("professional_id")) return "Professional ID";
            if (constraintName.Contains("profession_id")) return "Profession ID";
            if (constraintName.Contains("service_id")) return "Service ID";

            return GetConstraintEntityName(constraintName);
        }

        private static string GetUniqueConstraintFieldValue(PostgresException exception)
        {
            var detail = exception.Detail ?? string.Empty;
            var startIndex = detail.IndexOf("=(") + 2;
            var endIndex = detail.IndexOf(")");

            if (startIndex > 1 && endIndex > startIndex)
            {
                return detail.Substring(startIndex, endIndex - startIndex);
            }

            return "Duplicate value";
        }

        private class ErrorResponse
        {
            public int StatusCode { get; }
            public string Message { get; }
            public string Details { get; }
            public string Type { get; }

            public ErrorResponse(int statusCode, string message, string details)
            {
                StatusCode = statusCode;
                Message = message;
                Details = details;
                Type = "Error";
            }
        }
    }
}