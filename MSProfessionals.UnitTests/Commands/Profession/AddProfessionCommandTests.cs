using FluentAssertions;
using MSProfessionals.Application.Commands.Profession;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Profession;

public class AddProfessionCommandTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidParameters()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionName = "Médico";

        // Act
        var command = new AddProfessionCommand
        {
            ProfessionalId = professionalId,
            ProfessionName = professionName
        };

        // Assert
        command.Should().NotBeNull();
        command.ProfessionalId.Should().Be(professionalId);
        command.ProfessionName.Should().Be(professionName);
    }

    [Fact]
    public void Validate_ShouldThrowValidationException_WhenProfessionalIdIsEmpty()
    {
        // Arrange
        var command = new AddProfessionCommand
        {
            ProfessionalId = Guid.Empty,
            ProfessionName = "Médico"
        };

        // Act & Assert
        var validationContext = new ValidationContext(command);
        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateObject(command, validationContext, validateAllProperties: true));
        exception.Message.Should().Contain("Professional ID is required");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldThrowValidationException_WhenProfessionNameIsInvalid(string professionName)
    {
        // Arrange
        var command = new AddProfessionCommand
        {
            ProfessionalId = Guid.NewGuid(),
            ProfessionName = professionName
        };

        // Act & Assert
        var validationContext = new ValidationContext(command);
        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateObject(command, validationContext, validateAllProperties: true));
        exception.Message.Should().Contain("Profession name is required");
    }

    [Fact]
    public void Validate_ShouldThrowValidationException_WhenProfessionNameExceedsMaxLength()
    {
        // Arrange
        var command = new AddProfessionCommand
        {
            ProfessionalId = Guid.NewGuid(),
            ProfessionName = new string('a', 51) // Max length is 50
        };

        // Act & Assert
        var validationContext = new ValidationContext(command);
        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateObject(command, validationContext, validateAllProperties: true));
        exception.Message.Should().Contain("Profession name cannot exceed 50 characters");
    }
} 