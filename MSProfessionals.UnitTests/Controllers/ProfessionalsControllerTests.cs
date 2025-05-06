using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using Xunit;

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
    public async Task Create_ShouldReturnCreatedResult_WithCreatedProfessional()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "Test Professional",
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "test@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };

        var expectedResponse = new CreateProfessionalCommandResponse
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
            CreatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Create(command);

        // Assert
        var result = Assert.IsType<ActionResult<CreateProfessionalCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateProfessionalCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task Create_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            Name = "Test Professional",
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "test@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };
        var cancellationToken = new CancellationToken(true);

        var expectedResponse = new CreateProfessionalCommandResponse
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
            CreatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(m => m.Send(command, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Create(command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<CreateProfessionalCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateProfessionalCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithProfessional()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professional = new Professional
        {
            Id = id,
            Name = "Test Professional",
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "test@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var expectedResponse = new GetProfessionalByIdCommandResponse(professional);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalByIdCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetById(id);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalByIdCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionalByIdCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task GetById_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var professional = new Professional
        {
            Id = id,
            Name = "Test Professional",
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "test@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var expectedResponse = new GetProfessionalByIdCommandResponse(professional);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalByIdCommand>(cmd => cmd.Id == id), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetById(id, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalByIdCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionalByIdCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task GetByName_ShouldReturnOkResult_WithProfessional()
    {
        // Arrange
        var name = "Test Professional";
        var professional = new Professional
        {
            Id = Guid.NewGuid(),
            Name = name,
            DocumentId = "123456789",
            PhoneNumber = "1234567890",
            Email = "test@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var expectedResponse = new GetProfessionalByNameCommandResponse(professional);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalByNameCommand>(cmd => cmd.Name == name), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetProfessionalByNameCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task GetByName_ShouldReturnNotFound_WhenProfessionalNotFound()
    {
        // Arrange
        var name = "Nonexistent Professional";

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalByNameCommand>(cmd => cmd.Name == name), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProfessionalNotFoundException($"Professional with name {name} not found"));

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnOkResult_WithUpdatedProfessional()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = id,
            Name = "Updated Professional",
            DocumentId = "987654321",
            PhoneNumber = "0987654321",
            Email = "updated@example.com",
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

        var expectedResponse = new UpdateProfessionalCommandResponse(professional);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Update(id, command);

        // Assert
        var result = Assert.IsType<ActionResult<UpdateProfessionalCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UpdateProfessionalCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task Update_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateProfessionalCommand
        {
            Id = id,
            Name = "Updated Professional",
            DocumentId = "987654321",
            PhoneNumber = "0987654321",
            Email = "updated@example.com",
            CurrencyId = Guid.NewGuid(),
            PhoneCountryCodeId = Guid.NewGuid(),
            PreferredLanguageId = Guid.NewGuid(),
            TimezoneId = Guid.NewGuid()
        };
        var cancellationToken = new CancellationToken(true);

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

        var expectedResponse = new UpdateProfessionalCommandResponse(professional);

        _mediatorMock
            .Setup(m => m.Send(command, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Update(id, command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<UpdateProfessionalCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UpdateProfessionalCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.Name, response.Name);
        Assert.Equal(expectedResponse.Email, response.Email);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionalCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionalCommand>(cmd => cmd.Id == id), cancellationToken))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CreateProfession_ShouldReturnCreatedResult_WithCreatedProfession()
    {
        // Arrange
        var command = new CreateProfessionCommand
        {
            ProfessionalId = Guid.NewGuid(),
            ProfessionId = Guid.NewGuid()
        };

        var expectedResponse = new CreateProfessionCommandResponse(command.ProfessionalId, command.ProfessionId);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.CreateProfession(command);

        // Assert
        var result = Assert.IsType<ActionResult<CreateProfessionCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateProfessionCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.ProfessionalId, response.ProfessionalId);
        Assert.Equal(expectedResponse.ProfessionId, response.ProfessionId);
    }

    [Fact]
    public async Task CreateProfession_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var command = new CreateProfessionCommand
        {
            ProfessionalId = Guid.NewGuid(),
            ProfessionId = Guid.NewGuid()
        };
        var cancellationToken = new CancellationToken(true);

        var expectedResponse = new CreateProfessionCommandResponse(command.ProfessionalId, command.ProfessionId);

        _mediatorMock
            .Setup(m => m.Send(command, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.CreateProfession(command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<CreateProfessionCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateProfessionCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.ProfessionalId, response.ProfessionalId);
        Assert.Equal(expectedResponse.ProfessionId, response.ProfessionId);
    }

    [Fact]
    public async Task DeleteProfession_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteProfession(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProfession_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionCommand>(cmd => cmd.Id == id), cancellationToken))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteProfession(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetProfessionalProfessions_ShouldReturnOkResult_WithProfessions()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var expectedResponse = new List<GetProfessionalProfessionsCommandResponse>
        {
            new(professionId, professionalId, "Test Profession")
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalProfessionsCommand>(cmd => cmd.ProfessionalId == professionalId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetProfessionalProfessions(professionalId);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<GetProfessionalProfessionsCommandResponse>>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<GetProfessionalProfessionsCommandResponse>>(okResult.Value);
        Assert.Single(response);
        Assert.Equal(expectedResponse[0].ProfessionId, response[0].ProfessionId);
        Assert.Equal(expectedResponse[0].ProfessionalId, response[0].ProfessionalId);
        Assert.Equal(expectedResponse[0].Name, response[0].Name);
    }

    [Fact]
    public async Task GetProfessionalProfessions_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var expectedResponse = new List<GetProfessionalProfessionsCommandResponse>
        {
            new(professionId, professionalId, "Test Profession")
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalProfessionsCommand>(cmd => cmd.ProfessionalId == professionalId), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetProfessionalProfessions(professionalId, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<GetProfessionalProfessionsCommandResponse>>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<GetProfessionalProfessionsCommandResponse>>(okResult.Value);
        Assert.Single(response);
        Assert.Equal(expectedResponse[0].ProfessionId, response[0].ProfessionId);
        Assert.Equal(expectedResponse[0].ProfessionalId, response[0].ProfessionalId);
        Assert.Equal(expectedResponse[0].Name, response[0].Name);
    }

    [Fact]
    public async Task GetProfessionals_ShouldReturnOkResult_WithPaginatedProfessionals()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 20;

        var professionals = new List<ProfessionalItem>
        {
            new(
                Guid.NewGuid(),
                "Test Professional 1",
                "123456789",
                null,
                "1234567890",
                "test1@example.com",
                null,
                null,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow
            ),
            new(
                Guid.NewGuid(),
                "Test Professional 2",
                "987654321",
                null,
                "0987654321",
                "test2@example.com",
                null,
                null,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow
            )
        };

        var expectedResponse = new GetProfessionalsCommandResponse(pageNumber, pageSize, totalItems, professionals);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalsCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetProfessionals(pageNumber, pageSize);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalsCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionalsCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.PageNumber, response.PageNumber);
        Assert.Equal(expectedResponse.PageSize, response.PageSize);
        Assert.Equal(expectedResponse.TotalItems, response.TotalItems);
        Assert.Equal(expectedResponse.Items, response.Items);
    }

    [Fact]
    public async Task GetProfessionals_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 20;
        var cancellationToken = new CancellationToken(true);

        var professionals = new List<ProfessionalItem>
        {
            new(
                Guid.NewGuid(),
                "Test Professional 1",
                "123456789",
                null,
                "1234567890",
                "test1@example.com",
                null,
                null,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow
            ),
            new(
                Guid.NewGuid(),
                "Test Professional 2",
                "987654321",
                null,
                "0987654321",
                "test2@example.com",
                null,
                null,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow
            )
        };

        var expectedResponse = new GetProfessionalsCommandResponse(pageNumber, pageSize, totalItems, professionals);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalsCommand>(cmd => 
                cmd.PageNumber == pageNumber && 
                cmd.PageSize == pageSize), 
                cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetProfessionals(pageNumber, pageSize, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalsCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionalsCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.PageNumber, response.PageNumber);
        Assert.Equal(expectedResponse.PageSize, response.PageSize);
        Assert.Equal(expectedResponse.TotalItems, response.TotalItems);
        Assert.Equal(expectedResponse.Items, response.Items);
    }
} 