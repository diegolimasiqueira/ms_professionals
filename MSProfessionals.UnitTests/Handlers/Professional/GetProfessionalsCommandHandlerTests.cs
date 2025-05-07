using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Professional;

public class GetProfessionalsCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalsCommandHandler _handler;

    public GetProfessionalsCommandHandlerTests()
    {
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalsCommandHandler(_professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedProfessionals()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 25;
        var professionals = new List<MSProfessionals.Domain.Entities.Professional>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Professional 1",
                Email = "professional1@example.com",
                PhoneNumber = "1234567890",
                DocumentId = "123456789",
                CurrencyId = Guid.NewGuid(),
                PhoneCountryCodeId = Guid.NewGuid(),
                PreferredLanguageId = Guid.NewGuid(),
                TimezoneId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Professional 2",
                Email = "professional2@example.com",
                PhoneNumber = "0987654321",
                DocumentId = "987654321",
                CurrencyId = Guid.NewGuid(),
                PhoneCountryCodeId = Guid.NewGuid(),
                PreferredLanguageId = Guid.NewGuid(),
                TimezoneId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var command = new GetProfessionalsCommand { PageNumber = pageNumber, PageSize = pageSize };

        _professionalRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionals);

        _professionalRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalItems);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalItems.Should().Be(totalItems);
        result.TotalPages.Should().Be((int)Math.Ceiling(totalItems / (double)pageSize));
        result.Items.Should().HaveCount(2);

        var firstProfessional = result.Items.First();
        firstProfessional.Name.Should().Be(professionals[0].Name);
        firstProfessional.Email.Should().Be(professionals[0].Email);
        firstProfessional.PhoneNumber.Should().Be(professionals[0].PhoneNumber);
        firstProfessional.DocumentId.Should().Be(professionals[0].DocumentId);
        firstProfessional.CurrencyId.Should().Be(professionals[0].CurrencyId);
        firstProfessional.PhoneCountryCodeId.Should().Be(professionals[0].PhoneCountryCodeId);
        firstProfessional.PreferredLanguageId.Should().Be(professionals[0].PreferredLanguageId);
        firstProfessional.TimezoneId.Should().Be(professionals[0].TimezoneId);
        firstProfessional.CreatedAt.Should().Be(professionals[0].CreatedAt);
        firstProfessional.UpdatedAt.Should().Be(professionals[0].UpdatedAt);
    }

    [Fact]
    public async Task Handle_WhenNoProfessionalsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var command = new GetProfessionalsCommand { PageNumber = pageNumber, PageSize = pageSize };

        _professionalRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.Professional>());

        _professionalRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenPageNumberIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetProfessionalsCommand { PageNumber = 0, PageSize = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPageSizeIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetProfessionalsCommand { PageNumber = 1, PageSize = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 