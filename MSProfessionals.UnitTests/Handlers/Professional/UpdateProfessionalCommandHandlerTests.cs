using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class UpdateProfessionalCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly UpdateProfessionalCommandHandler _handler;

    public UpdateProfessionalCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new UpdateProfessionalCommandHandler(_professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProfessionalExists_ShouldUpdateProfessional()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = professionalId,
            Name = "Updated Professional",
            Email = "updated@example.com",
            PhoneNumber = "9876543210",
            DocumentId = "987654321",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        var existingProfessional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Original Professional",
            Email = "original@example.com",
            PhoneNumber = "1234567890",
            DocumentId = "123456789",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProfessional);

        _professionalRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(professionalId);
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
        result.PhoneNumber.Should().Be(command.PhoneNumber);
        result.DocumentId.Should().Be(command.DocumentId);
        result.CurrencyId.Should().Be(command.CurrencyId);
        result.PhoneCountryCodeId.Should().Be(command.PhoneCountryCodeId);
        result.PreferredLanguageId.Should().Be(command.PreferredLanguageId);
        result.TimezoneId.Should().Be(command.TimezoneId);
        result.CreatedAt.Should().Be(existingProfessional.CreatedAt);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _professionalRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfessionalDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var command = new UpdateProfessionalCommand 
        { 
            Id = Guid.NewGuid(),
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            DocumentId = "123456789",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        _professionalRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldThrowException()
    {
        // Arrange
        var command = new UpdateProfessionalCommand 
        { 
            Id = Guid.NewGuid(),
            Name = "Test Professional",
            Email = "existing@example.com",
            PhoneNumber = "1234567890",
            DocumentId = "123456789",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        var existingProfessional = new MSProfessionals.Domain.Entities.Professional 
        { 
            Id = Guid.NewGuid(),
            Name = "Existing Professional",
            Email = "existing@example.com",
            PhoneNumber = "9876543210",
            DocumentId = "987654321",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        _professionalRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MSProfessionals.Domain.Entities.Professional { Id = command.Id });

        _professionalRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProfessional);

        // Act & Assert
        await Assert.ThrowsAsync<UniqueConstraintViolationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenEmailIsInvalid_ShouldThrowException()
    {
        // Arrange
        var command = new UpdateProfessionalCommand 
        { 
            Id = Guid.NewGuid(),
            Name = "Test Professional",
            Email = "invalid-email",
            PhoneNumber = "1234567890",
            DocumentId = "123456789",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 