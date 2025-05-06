using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.Service;
using Xunit;

namespace MSProfessionals.UnitTests.Controllers;

public class ServiceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ServiceController _controller;

    public ServiceControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ServiceController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WithDefaultParameters()
    {
        // Arrange
        var expectedCommand = new GetServicesCommand
        {
            PageNumber = 1,
            PageSize = 10,
            Name = null
        };

        var expectedResponse = new GetServicesCommandResponse();
        _mediatorMock.Setup(x => x.Send(It.Is<GetServicesCommand>(c =>
                c.PageNumber == expectedCommand.PageNumber &&
                c.PageSize == expectedCommand.PageSize &&
                c.Name == expectedCommand.Name),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(cancellationToken: CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expectedResponse, okResult.Value);
        _mediatorMock.Verify(x => x.Send(It.Is<GetServicesCommand>(c =>
                c.PageNumber == expectedCommand.PageNumber &&
                c.PageSize == expectedCommand.PageSize &&
                c.Name == expectedCommand.Name),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WithCustomParameters()
    {
        // Arrange
        const int pageNumber = 2;
        const int pageSize = 20;
        const string name = "Test Service";

        var expectedCommand = new GetServicesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Name = name
        };

        var expectedResponse = new GetServicesCommandResponse();
        _mediatorMock.Setup(x => x.Send(It.Is<GetServicesCommand>(c =>
                c.PageNumber == expectedCommand.PageNumber &&
                c.PageSize == expectedCommand.PageSize &&
                c.Name == expectedCommand.Name),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(pageNumber, pageSize, name, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expectedResponse, okResult.Value);
        _mediatorMock.Verify(x => x.Send(It.Is<GetServicesCommand>(c =>
                c.PageNumber == expectedCommand.PageNumber &&
                c.PageSize == expectedCommand.PageSize &&
                c.Name == expectedCommand.Name),
            It.IsAny<CancellationToken>()), Times.Once);
    }
} 