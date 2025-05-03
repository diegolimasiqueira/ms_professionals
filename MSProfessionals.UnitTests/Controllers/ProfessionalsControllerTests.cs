using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands;
using MSProfessionals.Domain.Exceptions;
using MediatR;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MSProfessionals.UnitTests.Controllers;

public class ProfessionalsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ProfessionalsController>> _loggerMock;
    private readonly ProfessionalsController _controller;

    public ProfessionalsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ProfessionalsController>>();
        _controller = new ProfessionalsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Create_ValidCommand_ReturnsCreated()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "John Doe",
            DocumentId = "123456789",
            PhoneNumber = "+5511999999999",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        var response = new CreateProfessionalCommandResponse
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            DocumentId = command.DocumentId,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
            CurrencyId = command.CurrencyId,
            PhoneCountryCodeId = command.PhoneCountryCodeId,
            PreferredLanguageId = command.PreferredLanguageId,
            TimezoneId = command.TimezoneId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ProfessionalsController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(response.Id, createdAtActionResult.RouteValues["id"]);
        Assert.Equal(response, createdAtActionResult.Value);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professional = new Professional
        {
            Id = id,
            Name = "John Doe",
            DocumentId = "123456789",
            PhoneNumber = "+5511999999999",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var response = new GetProfessionalByIdCommandResponse(professional);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByIdCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProfessionalNotFoundException($"Professional with ID {id} not found"));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetByName_WhenProfessionalExists_ReturnsOkResult()
    {
        // Arrange
        var name = "John Doe";
        var professional = new Professional
        {
            Id = Guid.NewGuid(),
            Name = name,
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var response = new GetProfessionalByNameCommandResponse(professional);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<GetProfessionalByNameCommandResponse>(okResult.Value);
        Assert.Equal(professional.Id, returnValue.Id);
        Assert.Equal(professional.Name, returnValue.Name);
        Assert.Equal(professional.Email, returnValue.Email);
        Assert.Equal(professional.PhoneNumber, returnValue.PhoneNumber);
    }

    [Fact]
    public async Task GetByName_WhenProfessionalDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var name = "John Doe";
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByNameCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProfessionalNotFoundException($"Professional with name {name} not found"));

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ValidCommand_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = id,
            Name = "John Doe Updated",
            DocumentId = "123456789",
            PhoneNumber = "+5511999999999",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        var professional = new Professional
        {
            Id = id,
            Name = command.Name,
            DocumentId = command.DocumentId,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
            CurrencyId = command.CurrencyId,
            PhoneCountryCodeId = command.PhoneCountryCodeId,
            PreferredLanguageId = command.PreferredLanguageId,
            TimezoneId = command.TimezoneId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var response = new UpdateProfessionalCommandResponse(professional);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Update_WithIdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe Updated",
            DocumentId = "123456789",
            PhoneNumber = "+5511999999999",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ID mismatch", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = id,
            Name = "John Doe Updated",
            DocumentId = "123456789",
            PhoneNumber = "+5511999999999",
            Email = "john@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProfessionalNotFoundException($"Professional with ID {id} not found"));

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WhenProfessionalExists_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenProfessionalDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProfessionalNotFoundException($"Professional with ID {id} not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
} 