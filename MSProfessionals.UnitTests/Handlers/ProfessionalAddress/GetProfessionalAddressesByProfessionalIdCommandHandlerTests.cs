using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalAddress;

public class GetProfessionalAddressesByProfessionalIdCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalAddressesByProfessionalIdCommandHandler _handler;

    public GetProfessionalAddressesByProfessionalIdCommandHandlerTests()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalAddressesByProfessionalIdCommandHandler(
            _addressRepositoryMock.Object,
            _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAddresses_WhenProfessionalExists()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalAddressesByProfessionalIdCommand { ProfessionalId = professionalId };

        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
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

        var addresses = new List<MSProfessionals.Domain.Entities.ProfessionalAddress>
        {
            new(
                professionalId,
                "Street 1",
                "City 1",
                "State 1",
                "12345",
                10.0,
                20.0,
                true,
                Guid.NewGuid()
            )
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new(
                professionalId,
                "Street 2",
                "City 2",
                "State 2",
                "67890",
                30.0,
                40.0,
                false,
                Guid.NewGuid()
            )
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, CancellationToken.None))
            .ReturnsAsync(professional);

        _addressRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId))
            .ReturnsAsync(addresses);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);

        // Verify first address
        Assert.Equal(addresses[0].Id, resultList[0].Id);
        Assert.Equal(addresses[0].ProfessionalId, resultList[0].ProfessionalId);
        Assert.Equal(addresses[0].StreetAddress, resultList[0].StreetAddress);
        Assert.Equal(addresses[0].City, resultList[0].City);
        Assert.Equal(addresses[0].State, resultList[0].State);
        Assert.Equal(addresses[0].PostalCode, resultList[0].PostalCode);
        Assert.Equal(addresses[0].Latitude, resultList[0].Latitude);
        Assert.Equal(addresses[0].Longitude, resultList[0].Longitude);
        Assert.Equal(addresses[0].IsDefault, resultList[0].IsDefault);
        Assert.Equal(addresses[0].CountryId, resultList[0].CountryId);
        Assert.Equal(addresses[0].CreatedAt, resultList[0].CreatedAt);
        Assert.Equal(addresses[0].UpdatedAt, resultList[0].UpdatedAt);

        // Verify second address
        Assert.Equal(addresses[1].Id, resultList[1].Id);
        Assert.Equal(addresses[1].ProfessionalId, resultList[1].ProfessionalId);
        Assert.Equal(addresses[1].StreetAddress, resultList[1].StreetAddress);
        Assert.Equal(addresses[1].City, resultList[1].City);
        Assert.Equal(addresses[1].State, resultList[1].State);
        Assert.Equal(addresses[1].PostalCode, resultList[1].PostalCode);
        Assert.Equal(addresses[1].Latitude, resultList[1].Latitude);
        Assert.Equal(addresses[1].Longitude, resultList[1].Longitude);
        Assert.Equal(addresses[1].IsDefault, resultList[1].IsDefault);
        Assert.Equal(addresses[1].CountryId, resultList[1].CountryId);
        Assert.Equal(addresses[1].CreatedAt, resultList[1].CreatedAt);
        Assert.Equal(addresses[1].UpdatedAt, resultList[1].UpdatedAt);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalNotFoundException_WhenProfessionalDoesNotExist()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalAddressesByProfessionalIdCommand { ProfessionalId = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, CancellationToken.None))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenProfessionalHasNoAddresses()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalAddressesByProfessionalIdCommand { ProfessionalId = professionalId };

        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
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

        _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId, CancellationToken.None))
            .ReturnsAsync(professional);

        _addressRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalAddress>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
} 