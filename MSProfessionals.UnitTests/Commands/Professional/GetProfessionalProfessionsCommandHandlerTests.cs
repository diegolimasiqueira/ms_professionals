using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;
using ProfessionalEntity = MSProfessionals.Domain.Entities.Professional;

namespace MSProfessionals.UnitTests.Commands.Professional;

public class GetProfessionalProfessionsCommandHandlerTests
{
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalProfessionsCommandHandler _handler;

    public GetProfessionalProfessionsCommandHandlerTests()
    {
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalProfessionsCommandHandler(
            _professionalProfessionRepositoryMock.Object,
            _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProfessions_WhenProfessionalExists()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var request = new GetProfessionalProfessionsCommand
        {
            ProfessionalId = professionalId
        };

        var professional = new ProfessionalEntity { Id = professionalId };
        var profession1Id = Guid.NewGuid();
        var profession2Id = Guid.NewGuid();
        var professionalProfessions = new List<ProfessionalProfession>
        {
            new ProfessionalProfession(professionalId, profession1Id) { Id = Guid.NewGuid(), Profession = new MSProfessionals.Domain.Entities.Profession { Id = profession1Id, Name = "Médico" } },
            new ProfessionalProfession(professionalId, profession2Id) { Id = Guid.NewGuid(), Profession = new MSProfessionals.Domain.Entities.Profession { Id = profession2Id, Name = "Dentista" } }
        };

        _professionalRepositoryMock
            .Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalProfessionRepositoryMock
            .Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfessions);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeInAscendingOrder(x => x.Name);
        result.Should().AllSatisfy(x => x.ProfessionalId.Should().Be(professionalId));
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenProfessionalNotFound()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var request = new GetProfessionalProfessionsCommand
        {
            ProfessionalId = professionalId
        };

        _professionalRepositoryMock
            .Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProfessionalEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProfessions()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var request = new GetProfessionalProfessionsCommand
        {
            ProfessionalId = professionalId
        };

        var professional = new ProfessionalEntity { Id = professionalId };

        _professionalRepositoryMock
            .Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalProfessionRepositoryMock
            .Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProfessionalProfession>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
} 