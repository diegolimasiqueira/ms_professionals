using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class GetProfessionalProfessionsCommandHandlerTests
{
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalProfessionsCommandHandler _handler;

    public GetProfessionalProfessionsCommandHandlerTests()
    {
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalProfessionsCommandHandler(_professionalProfessionRepositoryMock.Object, _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProfessionalExists_ShouldReturnProfessions()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            DocumentId = "123456789",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionalProfessions = new List<MSProfessionals.Domain.Entities.ProfessionalProfession>
        {
            new(professionalId, Guid.NewGuid())
            {
                Profession = new Profession { Id = Guid.NewGuid(), Name = "Profession 1" }
            },
            new(professionalId, Guid.NewGuid())
            {
                Profession = new Profession { Id = Guid.NewGuid(), Name = "Profession 2" }
            }
        };

        var command = new GetProfessionalProfessionsCommand { ProfessionalId = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfessions);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var firstProfession = result.First();
        firstProfession.ProfessionalId.Should().Be(professionalId);
        firstProfession.ProfessionId.Should().Be(professionalProfessions[0].ProfessionId);
        firstProfession.Name.Should().Be(professionalProfessions[0].Profession.Name);

        var secondProfession = result.Last();
        secondProfession.ProfessionalId.Should().Be(professionalId);
        secondProfession.ProfessionId.Should().Be(professionalProfessions[1].ProfessionId);
        secondProfession.Name.Should().Be(professionalProfessions[1].Profession.Name);
    }

    [Fact]
    public async Task Handle_WhenProfessionalDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional?)null);

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalProfessionRepositoryMock.Verify(x => x.GetByProfessionalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProfessionalIdIsEmpty_ShouldThrowException()
    {
        // Arrange
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = Guid.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _professionalProfessionRepositoryMock.Verify(x => x.GetByProfessionalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 