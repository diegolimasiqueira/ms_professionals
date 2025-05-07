using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class GetProfessionalProfessionsCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly GetProfessionalProfessionsCommandHandler _handler;

    public GetProfessionalProfessionsCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _handler = new GetProfessionalProfessionsCommandHandler(
            _professionalProfessionRepositoryMock.Object,
            _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProfessions_WhenProfessionalExists()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = professionalId };

        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "+5511999999999",
            DocumentId = "12345678901",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionId = Guid.NewGuid();
        var professions = new List<MSProfessionals.Domain.Entities.ProfessionalProfession>
        {
            new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
            {
                Id = Guid.NewGuid(),
                Profession = new MSProfessionals.Domain.Entities.Profession
                {
                    Id = professionId,
                    Name = "Test Profession"
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professions);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var firstResult = result.First();
        Assert.Equal(professionId, firstResult.ProfessionId);
        Assert.Equal(professions[0].Profession.Name, firstResult.Name);
        Assert.Equal(professionalId, firstResult.ProfessionalId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenProfessionalHasNoProfessions()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = professionalId };

        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "+5511999999999",
            DocumentId = "12345678901",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalProfession>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenProfessionalDoesNotExist()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Professional with ID {professionalId} not found", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenProfessionalIdIsEmpty()
    {
        // Arrange
        var command = new GetProfessionalProfessionsCommand { ProfessionalId = Guid.Empty };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Professional ID cannot be empty", exception.Message);

        _professionalRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 