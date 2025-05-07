using MSProfessionals.Domain.Exceptions;
using Xunit;

namespace MSProfessionals.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WithNoParameters_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        Assert.Equal("Exception of type 'MSProfessionals.Domain.Exceptions.ValidationException' was thrown.", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new ValidationException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
    {
        // Arrange
        var message = "Test message";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new ValidationException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
} 