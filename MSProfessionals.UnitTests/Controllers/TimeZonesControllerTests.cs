using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.TimeZone;

namespace MSProfessionals.UnitTests.Controllers;

public class TimeZonesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TimeZonesController _controller;

    public TimeZonesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TimeZonesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetTimeZones_ShouldReturnOkResult_WithTimeZones()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 20;

        var timeZones = new List<TimeZoneItem>
        {
            new(Guid.NewGuid(), "UTC-03:00", "America/Sao_Paulo"),
            new(Guid.NewGuid(), "UTC+00:00", "Europe/London")
        };

        var expectedResponse = new GetTimeZonesCommandResponse(pageNumber, pageSize, totalItems, timeZones);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTimeZonesCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetTimeZones(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetTimeZonesCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.PageNumber, response.PageNumber);
        Assert.Equal(expectedResponse.PageSize, response.PageSize);
        Assert.Equal(expectedResponse.TotalItems, response.TotalItems);
        Assert.Equal(expectedResponse.Items, response.Items);
    }

    [Fact]
    public async Task GetTimeZones_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var cancellationToken = new CancellationToken(true);

        var timeZones = new List<TimeZoneItem>
        {
            new(Guid.NewGuid(), "UTC-03:00", "America/Sao_Paulo")
        };

        var expectedResponse = new GetTimeZonesCommandResponse(pageNumber, pageSize, 1, timeZones);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTimeZonesCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetTimeZones(pageNumber, pageSize, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetTimeZonesCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetTimeZonesByDescription_ShouldReturnOkResult_WithFilteredTimeZones()
    {
        // Arrange
        var description = "America";
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 5;

        var timeZones = new List<TimeZoneItem>
        {
            new(Guid.NewGuid(), "UTC-03:00", "America/Sao_Paulo"),
            new(Guid.NewGuid(), "UTC-04:00", "America/New_York")
        };

        var expectedResponse = new GetTimeZonesCommandResponse(pageNumber, pageSize, totalItems, timeZones);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetTimeZonesByDescriptionCommand>(cmd => 
                cmd.Description == description && 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetTimeZonesByDescription(description, pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetTimeZonesCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.PageNumber, response.PageNumber);
        Assert.Equal(expectedResponse.PageSize, response.PageSize);
        Assert.Equal(expectedResponse.TotalItems, response.TotalItems);
        Assert.Equal(expectedResponse.Items, response.Items);
    }

    [Fact]
    public async Task GetTimeZonesByDescription_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var description = "Europe";
        var pageNumber = 1;
        var pageSize = 10;
        var cancellationToken = new CancellationToken(true);

        var timeZones = new List<TimeZoneItem>
        {
            new(Guid.NewGuid(), "UTC+00:00", "Europe/London")
        };

        var expectedResponse = new GetTimeZonesCommandResponse(pageNumber, pageSize, 1, timeZones);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTimeZonesByDescriptionCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetTimeZonesByDescription(description, pageNumber, pageSize, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetTimeZonesCommandResponse>(okResult.Value);
    }
} 