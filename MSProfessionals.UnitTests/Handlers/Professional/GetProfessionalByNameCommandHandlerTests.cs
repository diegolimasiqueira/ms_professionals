using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class GetProfessionalByNameCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalByNameCommandHandler _handler;

    public GetProfessionalByNameCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalByNameCommandHandler(_professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProfessionalExists_ShouldReturnProfessional()
    {
        // Arrange
        var professionalName = "Test Professional";
        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = Guid.NewGuid(),
            Name = professionalName,
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
        var command = new GetProfessionalByNameCommand { Name = professionalName };

        _professionalRepositoryMock.Setup(x => x.GetByNameAsync(professionalName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(professionalName);
        result.Email.Should().Be(professional.Email);
        result.PhoneNumber.Should().Be(professional.PhoneNumber);
        result.DocumentId.Should().Be(professional.DocumentId);
        result.CurrencyId.Should().Be(professional.CurrencyId);
        result.PhoneCountryCodeId.Should().Be(professional.PhoneCountryCodeId);
        result.PreferredLanguageId.Should().Be(professional.PreferredLanguageId);
        result.TimezoneId.Should().Be(professional.TimezoneId);
        result.CreatedAt.Should().Be(professional.CreatedAt);
        result.UpdatedAt.Should().Be(professional.UpdatedAt);
    }

    [Fact]
    public async Task Handle_WhenProfessionalDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var professionalName = "Non Existent Professional";
        var command = new GetProfessionalByNameCommand { Name = professionalName };

        _professionalRepositoryMock.Setup(x => x.GetByNameAsync(professionalName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenNameIsEmpty_ShouldThrowException()
    {
        // Arrange
        var command = new GetProfessionalByNameCommand { Name = string.Empty };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        Assert.Contains("Professional name cannot be empty", exception.InnerException?.Message);
    }
} 