using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class DeleteProfessionalCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly DeleteProfessionalCommandHandler _handler;

    public DeleteProfessionalCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new DeleteProfessionalCommandHandler(_professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteProfessional_WhenValidCommand()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new DeleteProfessionalCommand { Id = professionalId };
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _professionalRepositoryMock.Verify(x => x.DeleteAsync(professionalId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalNotFoundException_WhenProfessionalDoesNotExist()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new DeleteProfessionalCommand { Id = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenIdIsEmpty()
    {
        // Arrange
        var command = new DeleteProfessionalCommand { Id = Guid.Empty };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Professional ID cannot be empty", exception.Message);

        _professionalRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 