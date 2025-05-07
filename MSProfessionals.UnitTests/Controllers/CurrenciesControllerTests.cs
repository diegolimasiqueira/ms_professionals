using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.Currency;

namespace MSProfessionals.UnitTests.Controllers;

public class CurrenciesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CurrenciesController _controller;

    public CurrenciesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CurrenciesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetCurrencies_ShouldReturnOkResult_WithPaginatedList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 2;
        var items = new List<CurrencyItem>
        {
            new(Guid.NewGuid(), "BRL", "Real Brasileiro"),
            new(Guid.NewGuid(), "USD", "Dólar Americano")
        };

        var expectedResponse = new GetCurrenciesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCurrenciesCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrencies(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Equal(items.Count, response.Items.Count());
    }

    [Fact]
    public async Task GetCurrencies_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var expectedResponse = new GetCurrenciesCommandResponse(1, 10, 0, new List<CurrencyItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCurrenciesCommand>(cmd => 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrencies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetCurrenciesByDescription_ShouldReturnOkResult_WithFilteredList()
    {
        // Arrange
        var description = "Real";
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 1;
        var items = new List<CurrencyItem>
        {
            new(Guid.NewGuid(), "BRL", "Real Brasileiro")
        };

        var expectedResponse = new GetCurrenciesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCurrenciesByDescriptionCommand>(cmd => 
                cmd.Description == description && 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrenciesByDescription(description, pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Single(response.Items);
        Assert.Contains(response.Items, item => item.Description.Contains(description));
    }

    [Fact]
    public async Task GetCurrenciesByDescription_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var description = "Real";
        var expectedResponse = new GetCurrenciesCommandResponse(1, 10, 0, new List<CurrencyItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCurrenciesByDescriptionCommand>(cmd => 
                cmd.Description == description && 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrenciesByDescription(description);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetCurrencies_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetCurrenciesCommandResponse(1, 10, 0, new List<CurrencyItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCurrenciesCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrencies(cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetCurrenciesByDescription_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetCurrenciesCommandResponse(1, 10, 0, new List<CurrencyItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCurrenciesByDescriptionCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetCurrenciesByDescription("Real", cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetCurrenciesCommandResponse>(okResult.Value);
    }
} 