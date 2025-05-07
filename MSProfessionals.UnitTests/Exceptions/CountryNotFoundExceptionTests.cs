using MSProfessionals.Domain.Exceptions;
using Xunit;

namespace MSProfessionals.UnitTests.Exceptions;

public class CountryNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new CountryNotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithNullMessage_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new CountryNotFoundException(null);

        // Assert
        Assert.Equal("Country not found", exception.Message);
    }
} 