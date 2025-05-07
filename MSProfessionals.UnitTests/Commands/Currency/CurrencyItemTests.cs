using FluentAssertions;
using MSProfessionals.Application.Commands.Currency;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Currency;

public class CurrencyItemTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "BRL";
        var description = "Real Brasileiro";

        // Act
        var currencyItem = new CurrencyItem(id, code, description);

        // Assert
        currencyItem.Should().NotBeNull();
        currencyItem.Id.Should().Be(id);
        currencyItem.Code.Should().Be(code);
        currencyItem.Description.Should().Be(description);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        string code = null!;
        var description = "Real Brasileiro";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new CurrencyItem(id, code, description));
        exception.ParamName.Should().Be("code");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDescriptionIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "BRL";
        string description = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new CurrencyItem(id, code, description));
        exception.ParamName.Should().Be("description");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowArgumentException_WhenCodeIsEmpty(string code)
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Real Brasileiro";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new CurrencyItem(id, code, description));
        exception.ParamName.Should().Be("code");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowArgumentException_WhenDescriptionIsEmpty(string description)
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "BRL";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new CurrencyItem(id, code, description));
        exception.ParamName.Should().Be("description");
    }
} 