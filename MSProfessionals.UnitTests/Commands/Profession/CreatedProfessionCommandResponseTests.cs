using FluentAssertions;
using MSProfessionals.Application.Commands.Profession;
using MSProfessionals.Domain.Entities;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Profession;

public class CreatedProfessionCommandResponseTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidParameters()
    {
        // Arrange
        var profession = new MSProfessionals.Domain.Entities.Profession
        {
            Id = Guid.NewGuid(),
            Name = "Médico"
        };

        // Act
        var response = new CreatedProfessionCommandResponse(profession);

        // Assert
        response.Should().NotBeNull();
        response.Profession.Should().Be(profession);
        response.Profession.Id.Should().Be(profession.Id);
        response.Profession.Name.Should().Be(profession.Name);
    }
} 