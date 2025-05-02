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

namespace MSProfessionals.UnitTests.Controllers;

public class ProfessionalsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProfessionalsController _controller;

    public ProfessionalsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProfessionalsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResult()
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

        var professional = new Professional
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
            .ReturnsAsync(professional);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ProfessionalsController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(professional.Id, createdAtActionResult.RouteValues["id"]);
        Assert.Equal(professional, createdAtActionResult.Value);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
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

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(professional, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Professional)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByName_WithValidName_ReturnsOkResult()
    {
        // Arrange
        var name = "John Doe";
        var professional = new Professional
        {
            Id = Guid.NewGuid(),
            Name = name,
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

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(professional, okResult.Value);
    }

    [Fact]
    public async Task GetByName_WithInvalidName_ReturnsNotFound()
    {
        // Arrange
        var name = "Non Existent";
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProfessionalByNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Professional)null);

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsOkResult()
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

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(professional, okResult.Value);
    }

    [Fact]
    public async Task Update_WithIdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = Guid.NewGuid(), // Different ID
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
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
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
            .ReturnsAsync((Professional)null);

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProfessionalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
} 