using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Exceptions;

public class ProfessionalServiceNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new ProfessionalServiceNotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithNullMessage_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new ProfessionalServiceNotFoundException(null);

        // Assert
        Assert.Equal("Professional service not found", exception.Message);
    }
} 