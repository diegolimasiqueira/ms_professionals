using FluentAssertions;
using MSProfessionals.Application.Commands.Language;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Language;

public class LanguageItemTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "pt-BR";
        var description = "Português (Brasil)";

        // Act
        var languageItem = new LanguageItem(id, code, description);

        // Assert
        languageItem.Should().NotBeNull();
        languageItem.Id.Should().Be(id);
        languageItem.Code.Should().Be(code);
        languageItem.Description.Should().Be(description);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        string code = null!;
        var description = "Português (Brasil)";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new LanguageItem(id, code, description));
        exception.ParamName.Should().Be("code");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDescriptionIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "pt-BR";
        string description = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new LanguageItem(id, code, description));
        exception.ParamName.Should().Be("description");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowArgumentException_WhenCodeIsEmpty(string code)
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Português (Brasil)";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new LanguageItem(id, code, description));
        exception.ParamName.Should().Be("code");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowArgumentException_WhenDescriptionIsEmpty(string description)
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "pt-BR";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new LanguageItem(id, code, description));
        exception.ParamName.Should().Be("description");
    }
} 