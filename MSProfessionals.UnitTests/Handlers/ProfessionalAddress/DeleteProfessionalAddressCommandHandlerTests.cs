using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalAddress;

public class DeleteProfessionalAddressCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly DeleteProfessionalAddressCommandHandler _handler;

    public DeleteProfessionalAddressCommandHandlerTests()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _handler = new DeleteProfessionalAddressCommandHandler(_addressRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAddress_WhenAddressExists()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var command = new DeleteProfessionalAddressCommand { Id = addressId };

        var address = new MSProfessionals.Domain.Entities.ProfessionalAddress(
            Guid.NewGuid(),
            "Street",
            "City",
            "State",
            "12345",
            null,
            null,
            false,
            Guid.NewGuid()
        )
        {
            Id = addressId
        };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync(address);

        _addressRepositoryMock.Setup(x => x.DeleteAsync(address))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _addressRepositoryMock.Verify(x => x.DeleteAsync(address), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalNotFoundException_WhenAddressDoesNotExist()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var command = new DeleteProfessionalAddressCommand { Id = addressId };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalAddress)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var command = new DeleteProfessionalAddressCommand { Id = Guid.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenForeignKeyViolation()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var command = new DeleteProfessionalAddressCommand { Id = addressId };

        var address = new MSProfessionals.Domain.Entities.ProfessionalAddress(
            Guid.NewGuid(),
            "Street",
            "City",
            "State",
            "12345",
            null,
            null,
            false,
            Guid.NewGuid()
        )
        {
            Id = addressId
        };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync(address);

        _addressRepositoryMock.Setup(x => x.DeleteAsync(address))
            .ThrowsAsync(new DbUpdateException("", new PostgresException("", "", "", "23503")));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        Assert.Contains("because it has associated records", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenUniqueConstraintViolation()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var command = new DeleteProfessionalAddressCommand { Id = addressId };

        var address = new MSProfessionals.Domain.Entities.ProfessionalAddress(
            Guid.NewGuid(),
            "Street",
            "City",
            "State",
            "12345",
            null,
            null,
            false,
            Guid.NewGuid()
        )
        {
            Id = addressId
        };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync(address);

        _addressRepositoryMock.Setup(x => x.DeleteAsync(address))
            .ThrowsAsync(new DbUpdateException("", new PostgresException("", "", "", "23505")));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        Assert.Contains("because it violates a unique constraint", exception.Message);
    }
} 