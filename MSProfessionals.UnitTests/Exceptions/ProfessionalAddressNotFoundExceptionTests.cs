using MSProfessionals.Application.Common.Exceptions;

namespace MSProfessionals.UnitTests.Exceptions;

public class ProfessionalAddressNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithId_ShouldSetMessage()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var exception = new ProfessionalAddressNotFoundException(id);

        // Assert
        Assert.Equal($"Professional address with ID {id} was not found.", exception.Message);
    }
} 