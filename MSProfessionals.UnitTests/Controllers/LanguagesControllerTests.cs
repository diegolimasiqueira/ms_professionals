using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.Language;

namespace MSProfessionals.UnitTests.Controllers;

public class LanguagesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly LanguagesController _controller;

    public LanguagesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new LanguagesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetLanguages_ShouldReturnOkResult_WithPaginatedList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 2;
        var items = new List<LanguageItem>
        {
            new(Guid.NewGuid(), "pt-BR", "Português do Brasil"),
            new(Guid.NewGuid(), "en-US", "English")
        };

        var expectedResponse = new GetLanguagesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetLanguagesCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguages(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Equal(items.Count, response.Items.Count());
    }

    [Fact]
    public async Task GetLanguages_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var expectedResponse = new GetLanguagesCommandResponse(1, 10, 0, new List<LanguageItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetLanguagesCommand>(cmd => 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguages();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetLanguagesByDescription_ShouldReturnOkResult_WithFilteredList()
    {
        // Arrange
        var description = "Português";
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 1;
        var items = new List<LanguageItem>
        {
            new(Guid.NewGuid(), "pt-BR", "Português do Brasil")
        };

        var expectedResponse = new GetLanguagesCommandResponse(pageNumber, pageSize, totalItems, items);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetLanguagesByDescriptionCommand>(cmd => 
                cmd.Description == description && 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguagesByDescription(description, pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalItems, response.TotalItems);
        Assert.Single(response.Items);
        Assert.Contains(response.Items, item => item.Description.Contains(description));
    }

    [Fact]
    public async Task GetLanguagesByDescription_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var description = "Português";
        var expectedResponse = new GetLanguagesCommandResponse(1, 10, 0, new List<LanguageItem>());

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetLanguagesByDescriptionCommand>(cmd => 
                cmd.Description == description && 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguagesByDescription(description);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task GetLanguages_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetLanguagesCommandResponse(1, 10, 0, new List<LanguageItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetLanguagesCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguages(cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetLanguagesByDescription_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetLanguagesCommandResponse(1, 10, 0, new List<LanguageItem>());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetLanguagesByDescriptionCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetLanguagesByDescription("Português", cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetLanguagesCommandResponse>(okResult.Value);
    }
} 