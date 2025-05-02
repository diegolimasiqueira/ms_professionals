using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred. Please try again later.",
                Details = exception.Message
            };

            switch (exception)
            {
                case ProfessionalNotFoundException professionalNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Professional not found",
                        Details = professionalNotFoundException.Message
                    };
                    break;

                case DbUpdateException dbUpdateException:
                    if (dbUpdateException.InnerException is PostgresException postgresException)
                    {
                        switch (postgresException.SqlState)
                        {
                            case "23503": // Foreign key violation
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = "Invalid reference data",
                                    Details = GetForeignKeyErrorMessage(postgresException)
                                };
                                break;
                            case "23505": // Unique constraint violation
                                var fieldName = GetUniqueConstraintFieldName(postgresException);
                                var fieldValue = GetUniqueConstraintFieldValue(postgresException);
                                
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.Conflict,
                                    Message = "Unique constraint violation",
                                    Details = new UniqueConstraintViolationException(fieldName, fieldValue).Message
                                };
                                break;
                            case "42703": // Column does not exist
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = "Invalid column reference",
                                    Details = $"The column '{GetColumnNameFromMessage(postgresException.Message)}' does not exist in the table"
                                };
                                break;
                            case "23502": // Not null violation
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = "Required field missing",
                                    Details = $"The field '{GetColumnNameFromMessage(postgresException.Message)}' cannot be null"
                                };
                                break;
                            case "22P02": // Invalid text representation
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = "Invalid data format",
                                    Details = "One or more fields contain invalid data format"
                                };
                                break;
                            case "23514": // Check violation
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                response = new
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = "Data validation failed",
                                    Details = GetCheckViolationMessage(postgresException)
                                };
                                break;
                        }
                    }
                    else if (dbUpdateException.InnerException != null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response = new
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Database operation failed",
                            Details = dbUpdateException.InnerException.Message
                        };
                    }
                    break;
            }

            _logger.LogError(exception, "Error processing request: {Message}", exception.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private string GetColumnNameFromMessage(string message)
        {
            // The message is something like: "column "CountryCodeId" of relation "tb_consumer_address" does not exist"
            var startIndex = message.IndexOf('"', message.IndexOf("column")) + 1;
            var endIndex = message.IndexOf('"', startIndex);
            
            if (startIndex > 0 && endIndex > startIndex)
            {
                return message.Substring(startIndex, endIndex - startIndex);
            }
            
            return "Unknown";
        }

        private string GetForeignKeyErrorMessage(PostgresException exception)
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
            
            return "The provided reference data is invalid";
        }

        private string GetUniqueConstraintFieldName(PostgresException exception)
        {
            var constraintName = exception.ConstraintName?.ToLower() ?? string.Empty;
            
            if (constraintName.Contains("email")) return "Email";
            if (constraintName.Contains("document_id")) return "Document ID";
            if (constraintName.Contains("phone_number")) return "Phone Number";
            
            return "Unique field";
        }

        private string GetUniqueConstraintFieldValue(PostgresException exception)
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

        private string GetCheckViolationMessage(PostgresException exception)
        {
            var constraintName = exception.ConstraintName?.ToLower() ?? string.Empty;
            
            if (constraintName.Contains("email_format")) 
                return "The email address format is invalid";
            if (constraintName.Contains("phone_format")) 
                return "The phone number format is invalid";
            if (constraintName.Contains("document_format")) 
                return "The document ID format is invalid";
            
            return "The data does not meet the required validation rules";
        }
    }
} 