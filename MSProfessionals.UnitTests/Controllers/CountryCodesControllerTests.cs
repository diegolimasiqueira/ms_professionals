using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.CountryCode;

namespace MSProfessionals.UnitTests.Controllers;

public class CountryCodesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CountryCodesController _controller;

    public CountryCodesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CountryCodesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetCountryCodes_ShouldReturnOkResult_WithPaginatedList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 2;
        var items = new List<CountryCodeItem>
        {
            new(Guid.NewGuid(), "BR", "Brasil"),
            new(Guid.NewGuid(), "US", "Estados Unidos")
        };

        var expectedResponse = new GetCountryCodesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCountryCodesCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodes(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Equal(items.Count, response.Items.Count());
    }

    [Fact]
    public async Task GetCountryCodes_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var expectedResponse = new GetCountryCodesCommandResponse(1, 10, 0, new List<CountryCodeItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCountryCodesCommand>(cmd => 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetCountryCodesByCountryName_ShouldReturnOkResult_WithFilteredList()
    {
        // Arrange
        var countryName = "Brasil";
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 1;
        var items = new List<CountryCodeItem>
        {
            new(Guid.NewGuid(), "BR", "Brasil")
        };

        var expectedResponse = new GetCountryCodesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCountryCodesByCountryNameCommand>(cmd => 
                cmd.CountryName == countryName && 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodesByCountryName(countryName, pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Single(response.Items);
        Assert.Contains(response.Items, item => item.CountryName == countryName);
    }

    [Fact]
    public async Task GetCountryCodesByCountryName_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var countryName = "Brasil";
        var expectedResponse = new GetCountryCodesCommandResponse(1, 10, 0, new List<CountryCodeItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCountryCodesByCountryNameCommand>(cmd => 
                cmd.CountryName == countryName && 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodesByCountryName(countryName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetCountryCodes_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetCountryCodesCommandResponse(1, 10, 0, new List<CountryCodeItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCountryCodesCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodes(cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetCountryCodesByCountryName_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetCountryCodesCommandResponse(1, 10, 0, new List<CountryCodeItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCountryCodesByCountryNameCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCountryCodesByCountryName("Brasil", cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<GetCountryCodesCommandResponse>(okResult.Value);
    }
} 