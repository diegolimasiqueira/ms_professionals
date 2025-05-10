using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class CreateProfessionalCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly CreateProfessionalCommandHandler _handler;

    public CreateProfessionalCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new CreateProfessionalCommandHandler(_professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProfessionalDoesNotExist_ShouldCreateProfessional()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
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
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
        result.PhoneNumber.Should().Be(command.PhoneNumber);
        result.DocumentId.Should().Be(command.DocumentId);
        result.CurrencyId.Should().Be(command.CurrencyId);
        result.PhoneCountryCodeId.Should().Be(command.PhoneCountryCodeId);
        result.PreferredLanguageId.Should().Be(command.PreferredLanguageId);
        result.TimezoneId.Should().Be(command.TimezoneId);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfessionalExists_ShouldThrowException()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "Test Professional",
            Email = "test@example.com",
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
            Email = command.Email,
            PhoneNumber = "0987654321",
            DocumentId = "987654321",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProfessional);

        // Act & Assert
        await Assert.ThrowsAsync<UniqueConstraintViolationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEmailIsInvalid_ShouldThrowException()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
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
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _professionalRepositoryMock.Verify(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 