using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class GetProfessionalServicesByProfessionalIdCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
    private readonly GetProfessionalServicesByProfessionalIdCommandHandler _handler;

    public GetProfessionalServicesByProfessionalIdCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _professionalRepositoryMock = new Mock<IProfessionalRepository>();
        _handler = new GetProfessionalServicesByProfessionalIdCommandHandler(
            _professionalServiceRepositoryMock.Object,
            _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnServices_WhenProfessionalExists()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalServicesByProfessionalIdCommand { ProfessionalId = professionalId };
        var professional = new MSProfessionals.Domain.Entities.Professional
        {
            Id = professionalId,
            Name = "Test Professional",
            Email = "test@example.com",
            PhoneNumber = "+5511999999999",
            DocumentId = "12345678901",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var professionId = Guid.NewGuid();
        var professionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
        {
            Id = Guid.NewGuid(),
            Profession = new MSProfessionals.Domain.Entities.Profession
            {
                Id = professionId,
                Name = "Test Profession"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        professional.ProfessionalProfessions = new List<MSProfessionals.Domain.Entities.ProfessionalProfession> { professionalProfession };

        var service1Id = Guid.NewGuid();
        var service2Id = Guid.NewGuid();
        var services = new List<MSProfessionals.Domain.Entities.ProfessionalService>
        {
            new MSProfessionals.Domain.Entities.ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = professionalProfession.Id,
                ServiceId = service1Id,
                Service = new MSProfessionals.Domain.Entities.Service
                {
                    Id = service1Id,
                    Name = "Test Service 1"
                },
                ProfessionalProfession = professionalProfession,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new MSProfessionals.Domain.Entities.ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = professionalProfession.Id,
                ServiceId = service2Id,
                Service = new MSProfessionals.Domain.Entities.Service
                {
                    Id = service2Id,
                    Name = "Test Service 2"
                },
                ProfessionalProfession = professionalProfession,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _professionalRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<MSProfessionals.Domain.Entities.Professional, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAsync(professionalProfession.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(services);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(services[0].Id, result.ElementAt(0).Id);
        Assert.Equal(services[0].ProfessionalProfessionId, result.ElementAt(0).ProfessionalProfessionId);
        Assert.Equal(services[0].ServiceId, result.ElementAt(0).ServiceId);
        Assert.Equal(services[0].Service.Name, result.ElementAt(0).ServiceName);
        Assert.Equal(services[0].ProfessionalProfession.Profession.Name, result.ElementAt(0).ProfessionName);
        Assert.Equal(services[1].Id, result.ElementAt(1).Id);
        Assert.Equal(services[1].ProfessionalProfessionId, result.ElementAt(1).ProfessionalProfessionId);
        Assert.Equal(services[1].ServiceId, result.ElementAt(1).ServiceId);
        Assert.Equal(services[1].Service.Name, result.ElementAt(1).ServiceName);
        Assert.Equal(services[1].ProfessionalProfession.Profession.Name, result.ElementAt(1).ProfessionName);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalNotFoundException_WhenProfessionalDoesNotExist()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var command = new GetProfessionalServicesByProfessionalIdCommand { ProfessionalId = professionalId };

        _professionalRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<MSProfessionals.Domain.Entities.Professional, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalNotFoundException_WhenProfessionalIdIsEmpty()
    {
        // Arrange
        var command = new GetProfessionalServicesByProfessionalIdCommand { ProfessionalId = Guid.Empty };

        _professionalRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<MSProfessionals.Domain.Entities.Professional, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Professional)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 