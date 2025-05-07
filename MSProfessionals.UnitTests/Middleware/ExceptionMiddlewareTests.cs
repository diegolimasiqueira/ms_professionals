using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MSProfessionals.API.Middleware;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Middleware;

public class ExceptionMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;
    private readonly HttpContext _context;
    private RequestDelegate _next;
    private ExceptionMiddleware _middleware;

    public ExceptionMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
        _next = context => Task.CompletedTask;
        _middleware = new ExceptionMiddleware(_next);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleGenericException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _next = _ => throw exception;
        _middleware = new ExceptionMiddleware(_next);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody, GetJsonSerializerOptions());

        Assert.Equal((int)HttpStatusCode.InternalServerError, _context.Response.StatusCode);
        Assert.Equal("Internal Server Error", response?.Message);
        Assert.Equal("Test exception", response?.Details);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleProfessionalNotFoundException()
    {
        // Arrange
        var exception = new ProfessionalNotFoundException("Professional not found");
        _next = _ => throw exception;
        _middleware = new ExceptionMiddleware(_next);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody, GetJsonSerializerOptions());

        Assert.Equal((int)HttpStatusCode.NotFound, _context.Response.StatusCode);
        Assert.Equal("Professional not found", response?.Message);
        Assert.Equal("Professional not found", response?.Details);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleValidationException()
    {
        // Arrange
        var exception = new ValidationException("Invalid input");
        _next = _ => throw exception;
        _middleware = new ExceptionMiddleware(_next);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody, GetJsonSerializerOptions());

        Assert.Equal((int)HttpStatusCode.BadRequest, _context.Response.StatusCode);
        Assert.Equal("Invalid input", response?.Message);
        Assert.Equal("Invalid input", response?.Details);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleUnauthorizedAccessException()
    {
        // Arrange
        var exception = new UnauthorizedAccessException("Access denied");
        _next = _ => throw exception;
        _middleware = new ExceptionMiddleware(_next);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody, GetJsonSerializerOptions());

        Assert.Equal((int)HttpStatusCode.Unauthorized, _context.Response.StatusCode);
        Assert.Equal("Access denied", response?.Message);
        Assert.Equal("Access denied", response?.Details);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleNoException()
    {
        // Arrange
        var nextCalled = false;
        _next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        _middleware = new ExceptionMiddleware(_next);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(200, _context.Response.StatusCode);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public object? Errors { get; set; }
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
} 