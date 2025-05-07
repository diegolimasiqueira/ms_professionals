using MSProfessionals.Domain.Exceptions;
using Xunit;

namespace MSProfessionals.UnitTests.Exceptions;

public class UniqueConstraintViolationExceptionTests
{
    [Fact]
    public void Constructor_WithFieldNameAndValue_ShouldSetMessageAndProperties()
    {
        // Arrange
        var fieldName = "Email";
        var fieldValue = "test@example.com";

        // Act
        var exception = new UniqueConstraintViolationException(fieldName, fieldValue);

        // Assert
        Assert.Equal($"The {fieldName} '{fieldValue}' is already in use.", exception.Message);
        Assert.Equal(fieldName, exception.FieldName);
        Assert.Equal(fieldValue, exception.FieldValue);
    }

    [Fact]
    public void Constructor_WithEmptyFieldName_ShouldSetMessageAndProperties()
    {
        // Arrange
        var fieldName = "";
        var fieldValue = "test@example.com";

        // Act
        var exception = new UniqueConstraintViolationException(fieldName, fieldValue);

        // Assert
        Assert.Equal($"The {fieldName} '{fieldValue}' is already in use.", exception.Message);
        Assert.Equal(fieldName, exception.FieldName);
        Assert.Equal(fieldValue, exception.FieldValue);
    }

    [Fact]
    public void Constructor_WithEmptyFieldValue_ShouldSetMessageAndProperties()
    {
        // Arrange
        var fieldName = "Email";
        var fieldValue = "";

        // Act
        var exception = new UniqueConstraintViolationException(fieldName, fieldValue);

        // Assert
        Assert.Equal($"The {fieldName} '{fieldValue}' is already in use.", exception.Message);
        Assert.Equal(fieldName, exception.FieldName);
        Assert.Equal(fieldValue, exception.FieldValue);
    }
} 