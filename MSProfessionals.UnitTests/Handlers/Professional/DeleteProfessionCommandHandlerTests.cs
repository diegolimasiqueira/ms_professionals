using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class DeleteProfessionCommandHandlerTests
{
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly DeleteProfessionCommandHandler _handler;

    public DeleteProfessionCommandHandlerTests()
    {
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _handler = new DeleteProfessionCommandHandler(_professionalProfessionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProfessionalProfessionExists_ShouldDeleteProfessionalProfession()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var professionalProfession = new ProfessionalProfession(Guid.NewGuid(), Guid.NewGuid()) { Id = professionalProfessionId };
        var command = new DeleteProfessionCommand { Id = professionalProfessionId };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _professionalProfessionRepositoryMock.Verify(x => x.DeleteAsync(professionalProfession, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfessionalProfessionDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var command = new DeleteProfessionCommand { Id = professionalProfessionId };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProfessionalProfession?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalProfessionNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalProfessionRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<ProfessionalProfession>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIdIsEmpty_ShouldThrowException()
    {
        // Arrange
        var command = new DeleteProfessionCommand { Id = Guid.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalProfessionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _professionalProfessionRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<ProfessionalProfession>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 