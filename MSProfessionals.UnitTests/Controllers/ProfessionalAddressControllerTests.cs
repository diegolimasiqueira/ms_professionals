using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Controllers;

public class ProfessionalAddressControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProfessionalAddressController _controller;

    public ProfessionalAddressControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProfessionalAddressController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedResult_WithCreatedAddress()
    {
        // Arrange
        var command = new CreateProfessionalAddressCommand
        {
            ProfessionalId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "São Paulo",
            State = "SP",
            PostalCode = "01234-567",
            CountryId = Guid.NewGuid(),
            IsDefault = true
        };

        var professionalAddress = new ProfessionalAddress(
            command.ProfessionalId,
            command.StreetAddress,
            command.City,
            command.State,
            command.PostalCode,
            command.Latitude,
            command.Longitude,
            command.IsDefault,
            command.CountryId
        );

        var expectedResponse = new CreateProfessionalAddressCommandResponse(professionalAddress);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<CreateProfessionalAddressCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalId, response.ProfessionalId);
        Assert.Equal(expectedResponse.StreetAddress, response.StreetAddress);
        Assert.Equal(expectedResponse.City, response.City);
        Assert.Equal(expectedResponse.State, response.State);
        Assert.Equal(expectedResponse.PostalCode, response.PostalCode);
        Assert.Equal(expectedResponse.CountryId, response.CountryId);
        Assert.Equal(expectedResponse.IsDefault, response.IsDefault);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithAddress()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();

        var professionalAddress = new ProfessionalAddress(
            professionalId,
            "Rua Teste",
            "São Paulo",
            "SP",
            "01234-567",
            null,
            null,
            true,
            countryId
        );

        var expectedResponse = new GetProfessionalAddressByIdCommandResponse(professionalAddress);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalAddressByIdCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetProfessionalAddressByIdCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalId, response.ProfessionalId);
        Assert.Equal(expectedResponse.StreetAddress, response.StreetAddress);
        Assert.Equal(expectedResponse.City, response.City);
        Assert.Equal(expectedResponse.State, response.State);
        Assert.Equal(expectedResponse.PostalCode, response.PostalCode);
        Assert.Equal(expectedResponse.CountryId, response.CountryId);
        Assert.Equal(expectedResponse.IsDefault, response.IsDefault);
    }

    [Fact]
    public async Task GetByProfessionalId_ShouldReturnOkResult_WithAddresses()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();

        var addresses = new List<ProfessionalAddress>
        {
            new(
                professionalId,
                "Rua Teste 1",
                "São Paulo",
                "SP",
                "01234-567",
                null,
                null,
                true,
                countryId
            ),
            new(
                professionalId,
                "Rua Teste 2",
                "Rio de Janeiro",
                "RJ",
                "20000-000",
                null,
                null,
                false,
                countryId
            )
        };

        var expectedResponse = addresses.Select(a => new GetProfessionalAddressByIdCommandResponse(a)).ToList();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalAddressesByProfessionalIdCommand>(cmd => cmd.ProfessionalId == professionalId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetByProfessionalId(professionalId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<GetProfessionalAddressByIdCommandResponse>>(okResult.Value);
        Assert.Equal(2, response.Count);
        Assert.All(response, item => Assert.Equal(professionalId, item.ProfessionalId));
    }

    [Fact]
    public async Task Update_ShouldReturnOkResult_WithUpdatedAddress()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();

        var command = new UpdateProfessionalAddressCommand
        {
            Id = id,
            StreetAddress = "Rua Teste Atualizada",
            City = "São Paulo",
            State = "SP",
            PostalCode = "01234-567",
            CountryId = countryId,
            IsDefault = true
        };

        var professionalAddress = new ProfessionalAddress(
            professionalId,
            command.StreetAddress,
            command.City,
            command.State,
            command.PostalCode,
            command.Latitude,
            command.Longitude,
            command.IsDefault,
            command.CountryId
        );

        var expectedResponse = new UpdateProfessionalAddressCommandResponse(professionalAddress);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<UpdateProfessionalAddressCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalId, response.ProfessionalId);
        Assert.Equal(expectedResponse.StreetAddress, response.StreetAddress);
        Assert.Equal(expectedResponse.City, response.City);
        Assert.Equal(expectedResponse.State, response.State);
        Assert.Equal(expectedResponse.PostalCode, response.PostalCode);
        Assert.Equal(expectedResponse.CountryId, response.CountryId);
        Assert.Equal(expectedResponse.IsDefault, response.IsDefault);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionalAddressCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetById_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var professionalAddress = new ProfessionalAddress(
            professionalId,
            "Rua Teste",
            "São Paulo",
            "SP",
            "01234-567",
            null,
            null,
            true,
            countryId
        );

        var expectedResponse = new GetProfessionalAddressByIdCommandResponse(professionalAddress);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfessionalAddressByIdCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(id, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetProfessionalAddressByIdCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetByProfessionalId_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var addresses = new List<ProfessionalAddress>
        {
            new(
                professionalId,
                "Rua Teste 1",
                "São Paulo",
                "SP",
                "01234-567",
                null,
                null,
                true,
                countryId
            )
        };

        var expectedResponse = addresses.Select(a => new GetProfessionalAddressByIdCommandResponse(a)).ToList();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfessionalAddressesByProfessionalIdCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetByProfessionalId(professionalId, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<GetProfessionalAddressByIdCommandResponse>>(okResult.Value);
    }

    [Fact]
    public async Task Update_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var command = new UpdateProfessionalAddressCommand
        {
            Id = id,
            StreetAddress = "Rua Teste",
            City = "São Paulo",
            State = "SP",
            PostalCode = "01234-567",
            CountryId = countryId,
            IsDefault = true
        };

        var professionalAddress = new ProfessionalAddress(
            professionalId,
            command.StreetAddress,
            command.City,
            command.State,
            command.PostalCode,
            command.Latitude,
            command.Longitude,
            command.IsDefault,
            command.CountryId
        );

        var expectedResponse = new UpdateProfessionalAddressCommandResponse(professionalAddress);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateProfessionalAddressCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(command, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<UpdateProfessionalAddressCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task Delete_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteProfessionalAddressCommand>(), cancellationToken))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 