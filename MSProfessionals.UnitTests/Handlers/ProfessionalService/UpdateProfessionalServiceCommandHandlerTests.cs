using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Npgsql;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class UpdateProfessionalServiceCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly UpdateProfessionalServiceCommandHandler _handler;

    public UpdateProfessionalServiceCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _handler = new UpdateProfessionalServiceCommandHandler(
            _professionalServiceRepositoryMock.Object,
            _professionalProfessionRepositoryMock.Object,
            _serviceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateProfessionalService()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = command.Id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionalProfession = new ProfessionalProfession(command.ProfessionalProfessionId, Guid.NewGuid());
        var service = new MSProfessionals.Domain.Entities.Service { Id = command.ServiceId };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalService);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        Assert.Equal(command.ProfessionalProfessionId, result.ProfessionalProfessionId);
        Assert.Equal(command.ServiceId, result.ServiceId);
        Assert.NotNull(result.UpdatedAt);

        _professionalServiceRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MSProfessionals.Domain.Entities.ProfessionalService>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.Empty,
            ProfessionalProfessionId = Guid.Empty,
            ServiceId = Guid.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalServiceNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentProfessionalService_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalService)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalServiceNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentProfessionalProfession_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = command.Id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalService);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProfessionalProfession)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalProfessionNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentService_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = command.Id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionalProfession = new ProfessionalProfession(command.ProfessionalProfessionId, Guid.NewGuid());

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalService);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Service)null);

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithDbUpdateException_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new UpdateProfessionalServiceCommand
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = command.Id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionalProfession = new ProfessionalProfession(command.ProfessionalProfessionId, Guid.NewGuid());
        var service = new MSProfessionals.Domain.Entities.Service { Id = command.ServiceId };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalService);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _professionalServiceRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<MSProfessionals.Domain.Entities.ProfessionalService>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Database error", new PostgresException("Error", "ERROR", "ERROR", "23505")));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
} 