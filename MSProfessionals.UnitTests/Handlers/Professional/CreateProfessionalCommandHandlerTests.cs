using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using ValidationException = MSProfessionals.Domain.Exceptions.ValidationException;
using System.ComponentModel.DataAnnotations;

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
    public async Task Handle_ShouldCreateProfessional_WhenValidCommand()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "+5511999999999",
            DocumentId = "12345678901",
            PhotoUrl = "https://example.com/photo.jpg",
            SocialMedia = new Dictionary<string, string> { { "linkedin", "https://linkedin.com/test" } },
            Media = new Dictionary<string, string> { { "portfolio", "https://portfolio.com/test" } },
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        _professionalRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null);

        _professionalRepositoryMock.Setup(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()))
            .Callback<MSProfessionals.Domain.Entities.Professional, CancellationToken>((p, ct) =>
            {
                p.CreatedAt = DateTime.UtcNow;
                p.UpdatedAt = DateTime.UtcNow;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(command.PhoneNumber, result.PhoneNumber);
        Assert.Equal(command.DocumentId, result.DocumentId);
        Assert.Equal(command.PhotoUrl, result.PhotoUrl);
        Assert.Equal(command.SocialMedia, result.SocialMedia);
        Assert.Equal(command.Media, result.Media);
        Assert.Equal(command.CurrencyId, result.CurrencyId);
        Assert.Equal(command.PhoneCountryCodeId, result.PhoneCountryCodeId);
        Assert.Equal(command.PreferredLanguageId, result.PreferredLanguageId);
        Assert.Equal(command.TimezoneId, result.TimezoneId);
        Assert.NotEqual(DateTime.MinValue, result.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, result.UpdatedAt);

        _professionalRepositoryMock.Verify(x => x.AddAsync(It.Is<MSProfessionals.Domain.Entities.Professional>(p => 
            p.Name == command.Name &&
            p.Email == command.Email &&
            p.PhoneNumber == command.PhoneNumber &&
            p.DocumentId == command.DocumentId &&
            p.PhotoUrl == command.PhotoUrl &&
            p.SocialMedia == command.SocialMedia &&
            p.Media == command.Media &&
            p.CurrencyId == command.CurrencyId &&
            p.PhoneCountryCodeId == command.PhoneCountryCodeId &&
            p.PreferredLanguageId == command.PreferredLanguageId &&
            p.TimezoneId == command.TimezoneId &&
            p.CreatedAt != DateTime.MinValue &&
            p.UpdatedAt != DateTime.MinValue
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUniqueConstraintViolationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "+5511999999999",
            DocumentId = "12345678901",
            PhotoUrl = "https://example.com/photo.jpg",
            SocialMedia = new Dictionary<string, string> { { "linkedin", "https://linkedin.com/test" } },
            Media = new Dictionary<string, string> { { "portfolio", "https://portfolio.com/test" } },
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
            PhoneNumber = "+5511888888888",
            DocumentId = "98765432109",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProfessional);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UniqueConstraintViolationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        Assert.Equal("Email", exception.FieldName);
        Assert.Equal(command.Email, exception.FieldValue);

        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentNullException_WhenCommandIsNull()
    {
        // Arrange
        CreateProfessionalCommand command = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _handler.Handle(command, CancellationToken.None));

        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("", "test@example.com", "+5511999999999", "12345678901", "Name is required")]
    [InlineData("Test Professional", "", "+5511999999999", "12345678901", "Email is required")]
    [InlineData("Test Professional", "test@example.com", "", "12345678901", "Phone number is required")]
    [InlineData("Test Professional", "test@example.com", "+5511999999999", "", "Document ID is required")]
    public async Task Handle_ShouldThrowValidationException_WhenRequiredFieldsAreMissing(string name, string email, string phoneNumber, string documentId, string expectedError)
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            DocumentId = documentId,
            PhotoUrl = "https://example.com/photo.jpg",
            SocialMedia = new Dictionary<string, string> { { "linkedin", "https://linkedin.com/test" } },
            Media = new Dictionary<string, string> { { "portfolio", "https://portfolio.com/test" } },
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        Assert.Contains(expectedError, exception.Message);

        _professionalRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.Professional>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 