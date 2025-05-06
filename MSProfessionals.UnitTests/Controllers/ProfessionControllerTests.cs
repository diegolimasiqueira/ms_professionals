using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.Profession;
using Xunit;

namespace MSProfessionals.UnitTests.Controllers;

public class ProfessionControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProfessionController _controller;

    public ProfessionControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProfessionController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WithPaginatedList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalCount = 2;
        var items = new List<GetProfessionsCommandResponse.ProfessionItem>
        {
            new() { Id = Guid.NewGuid(), Name = "Médico" },
            new() { Id = Guid.NewGuid(), Name = "Dentista" }
        };

        var expectedResponse = new GetProfessionsCommandResponse
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 1
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionsCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize && 
                cmd.Name == null), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionsCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalCount, response.TotalCount);
        Assert.Equal(items.Count, response.Items.Count());
    }

    [Fact]
    public async Task Get_WithDefaultParameters_ShouldReturnOkResult()
    {
        // Arrange
        var expectedResponse = new GetProfessionsCommandResponse
        {
            Items = new List<GetProfessionsCommandResponse.ProfessionItem>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10,
            TotalPages = 0
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionsCommand>(cmd => 
                cmd.PageNumber == 1 && 
                cmd.PageSize == 10 && 
                cmd.Name == null), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionsCommandResponse>(okResult.Value);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(10, response.PageSize);
    }

    [Fact]
    public async Task Get_WithNameFilter_ShouldReturnOkResult_WithFilteredList()
    {
        // Arrange
        var name = "Médico";
        var pageNumber = 1;
        var pageSize = 10;
        var totalCount = 1;
        var items = new List<GetProfessionsCommandResponse.ProfessionItem>
        {
            new() { Id = Guid.NewGuid(), Name = "Médico" }
        };

        var expectedResponse = new GetProfessionsCommandResponse
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 1
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionsCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize && 
                cmd.Name == name), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(pageNumber, pageSize, name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionsCommandResponse>(okResult.Value);
        Assert.Equal(pageNumber, response.PageNumber);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(totalCount, response.TotalCount);
        Assert.Single(response.Items);
        Assert.Contains(response.Items, item => item.Name.Contains(name));
    }

    [Fact]
    public async Task Get_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new GetProfessionsCommandResponse
        {
            Items = new List<GetProfessionsCommandResponse.ProfessionItem>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10,
            TotalPages = 0
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfessionsCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(cancellationToken: cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<GetProfessionsCommandResponse>(okResult.Value);
    }
} 