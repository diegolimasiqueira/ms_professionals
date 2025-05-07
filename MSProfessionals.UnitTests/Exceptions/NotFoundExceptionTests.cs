using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithNullMessage_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new NotFoundException(null);

        // Assert
        Assert.Equal("Not found", exception.Message);
    }
} 