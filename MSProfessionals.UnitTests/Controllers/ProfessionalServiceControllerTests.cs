using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MSProfessionals.API.Controllers;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Entities;
using Xunit;

namespace MSProfessionals.UnitTests.Controllers;

public class ProfessionalServiceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProfessionalServiceController _controller;

    public ProfessionalServiceControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProfessionalServiceController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var professionalProfessionId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var professionalService = new ProfessionalService
        {
            Id = id,
            ProfessionalProfessionId = professionalProfessionId,
            ServiceId = serviceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, professionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new GetProfessionalServiceByIdCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalServiceByIdCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetById(id);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalServiceByIdCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetProfessionalServiceByIdCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalProfessionId, response.ProfessionalProfessionId);
        Assert.Equal(expectedResponse.ServiceId, response.ServiceId);
        Assert.Equal(expectedResponse.ServiceName, response.ServiceName);
        Assert.Equal(expectedResponse.ProfessionName, response.ProfessionName);
    }

    [Fact]
    public async Task GetByProfessionalId_ShouldReturnOkResult_WithServices()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionalProfessionId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var professionalService = new ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = professionalProfessionId,
            ServiceId = serviceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, professionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new List<GetProfessionalServiceByIdCommandResponse>
        {
            new(professionalService)
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProfessionalServicesByProfessionalIdCommand>(cmd => cmd.ProfessionalId == professionalId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetByProfessionalId(professionalId);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<GetProfessionalServiceByIdCommandResponse>>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<GetProfessionalServiceByIdCommandResponse>>(okResult.Value);
        Assert.Single(response);
        Assert.Equal(expectedResponse[0].Id, response[0].Id);
        Assert.Equal(expectedResponse[0].ProfessionalProfessionId, response[0].ProfessionalProfessionId);
        Assert.Equal(expectedResponse[0].ServiceId, response[0].ServiceId);
    }

    [Fact]
    public async Task Add_ShouldReturnCreatedResult_WithCreatedService()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new AddProfessionalServiceCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Add(command);

        // Assert
        var result = Assert.IsType<ActionResult<AddProfessionalServiceCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<AddProfessionalServiceCommandResponse>(createdResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalProfessionId, response.ProfessionalProfessionId);
        Assert.Equal(expectedResponse.ServiceId, response.ServiceId);
    }

    [Fact]
    public async Task Update_ShouldReturnOkResult_WithUpdatedService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var command = new UpdateProfessionalServiceCommand
        {
            Id = id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalService = new ProfessionalService
        {
            Id = id,
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new UpdateProfessionalServiceCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Update(id, command);

        // Assert
        var result = Assert.IsType<ActionResult<UpdateProfessionalServiceCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UpdateProfessionalServiceCommandResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.ProfessionalProfessionId, response.ProfessionalProfessionId);
        Assert.Equal(expectedResponse.ServiceId, response.ServiceId);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteProfessionalServiceCommand>(cmd => cmd.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddMultiple_ShouldReturnCreatedResult_WithCreatedServices()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new AddMultipleProfessionalServicesCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        var professionalServices = new List<ProfessionalService>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = command.ProfessionalProfessionId,
                ServiceId = command.ServiceIds.First(),
                Service = new Service { Name = "Test Service 1" },
                ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
                { 
                    Profession = new Profession { Name = "Test Profession" } 
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = command.ProfessionalProfessionId,
                ServiceId = command.ServiceIds.Last(),
                Service = new Service { Name = "Test Service 2" },
                ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
                { 
                    Profession = new Profession { Name = "Test Profession" } 
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var expectedResponse = professionalServices.Select(ps => new AddProfessionalServiceCommandResponse(ps)).ToList();

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.AddMultiple(command);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<AddProfessionalServiceCommandResponse>>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<List<AddProfessionalServiceCommandResponse>>(createdResult.Value);
        Assert.Equal(2, response.Count);
        Assert.Equal(expectedResponse[0].Id, response[0].Id);
        Assert.Equal(expectedResponse[1].Id, response[1].Id);
    }

    [Fact]
    public async Task GetById_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var professionalService = new ProfessionalService
        {
            Id = id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, Guid.NewGuid())
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new GetProfessionalServiceByIdCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfessionalServiceByIdCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetById(id, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<GetProfessionalServiceByIdCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<GetProfessionalServiceByIdCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task GetByProfessionalId_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        var professionalService = new ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, Guid.NewGuid())
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new List<GetProfessionalServiceByIdCommandResponse>
        {
            new(professionalService)
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfessionalServicesByProfessionalIdCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.GetByProfessionalId(professionalId, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<GetProfessionalServiceByIdCommandResponse>>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<List<GetProfessionalServiceByIdCommandResponse>>(okResult.Value);
    }

    [Fact]
    public async Task Add_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };
        var cancellationToken = new CancellationToken(true);

        var professionalService = new ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new AddProfessionalServiceCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddProfessionalServiceCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Add(command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<AddProfessionalServiceCommandResponse>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<AddProfessionalServiceCommandResponse>(createdResult.Value);
    }

    [Fact]
    public async Task Update_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var command = new UpdateProfessionalServiceCommand
        {
            Id = id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };
        var cancellationToken = new CancellationToken(true);

        var professionalService = new ProfessionalService
        {
            Id = id,
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceId,
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new UpdateProfessionalServiceCommandResponse(professionalService);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateProfessionalServiceCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.Update(id, command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<UpdateProfessionalServiceCommandResponse>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<UpdateProfessionalServiceCommandResponse>(okResult.Value);
    }

    [Fact]
    public async Task Delete_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken(true);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteProfessionalServiceCommand>(), cancellationToken))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddMultiple_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new AddMultipleProfessionalServicesCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceIds = new List<Guid> { Guid.NewGuid() }
        };
        var cancellationToken = new CancellationToken(true);

        var professionalService = new ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceIds.First(),
            Service = new Service { Name = "Test Service" },
            ProfessionalProfession = new ProfessionalProfession(professionalId, command.ProfessionalProfessionId)
            { 
                Profession = new Profession { Name = "Test Profession" } 
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var expectedResponse = new List<AddProfessionalServiceCommandResponse>
        {
            new(professionalService)
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddMultipleProfessionalServicesCommand>(), cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.AddMultiple(command, cancellationToken);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<AddProfessionalServiceCommandResponse>>>(actionResult);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<List<AddProfessionalServiceCommandResponse>>(createdResult.Value);
    }
} 