using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Application.Common.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalAddress;

public class GetProfessionalAddressByIdCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly GetProfessionalAddressByIdCommandHandler _handler;

    public GetProfessionalAddressByIdCommandHandlerTests()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _handler = new GetProfessionalAddressByIdCommandHandler(_addressRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAddress_WhenAddressExists()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var command = new GetProfessionalAddressByIdCommand { Id = addressId };

        var address = new MSProfessionals.Domain.Entities.ProfessionalAddress(
            professionalId,
            "Street",
            "City",
            "State",
            "12345",
            10.0,
            20.0,
            true,
            countryId
        )
        {
            Id = addressId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync(address);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressId, result.Id);
        Assert.Equal(professionalId, result.ProfessionalId);
        Assert.Equal("Street", result.StreetAddress);
        Assert.Equal("City", result.City);
        Assert.Equal("State", result.State);
        Assert.Equal("12345", result.PostalCode);
        Assert.Equal(10.0, result.Latitude);
        Assert.Equal(20.0, result.Longitude);
        Assert.True(result.IsDefault);
        Assert.Equal(countryId, result.CountryId);
        Assert.Equal(address.CreatedAt, result.CreatedAt);
        Assert.Equal(address.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalAddressNotFoundException_WhenAddressDoesNotExist()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var command = new GetProfessionalAddressByIdCommand { Id = addressId };

        _addressRepositoryMock.Setup(x => x.GetByIdAsync(addressId))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalAddress)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalAddressNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 