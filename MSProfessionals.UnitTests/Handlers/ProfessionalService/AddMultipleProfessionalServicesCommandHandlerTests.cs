using Moq;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class AddMultipleProfessionalServicesCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly AddMultipleProfessionalServicesCommandHandler _handler;

    public AddMultipleProfessionalServicesCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _handler = new AddMultipleProfessionalServicesCommandHandler(
            _professionalServiceRepositoryMock.Object,
            _professionalProfessionRepositoryMock.Object,
            _serviceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldAddProfessionalServices()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var serviceIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var command = new AddMultipleProfessionalServicesCommand { ProfessionalProfessionId = professionalProfessionId, ServiceIds = serviceIds };

        var professionalProfession = new ProfessionalProfession(professionalProfessionId, professionalId);
        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(professionalProfession);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, CancellationToken.None))
            .ReturnsAsync(new List<ProfessionalProfession> { professionalProfession });

        foreach (var serviceId in serviceIds)
        {
            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId, CancellationToken.None))
                .ReturnsAsync(new MSProfessionals.Domain.Entities.Service { Id = serviceId });
        }

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalService>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(serviceIds.Count, result.Count());
        _professionalServiceRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<MSProfessionals.Domain.Entities.ProfessionalService>(), CancellationToken.None),
            Times.Exactly(serviceIds.Count));
    }

    [Fact]
    public async Task Handle_WithInvalidProfessionalProfessionId_ShouldThrowException()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var serviceIds = new List<Guid> { Guid.NewGuid() };
        var command = new AddMultipleProfessionalServicesCommand { ProfessionalProfessionId = professionalProfessionId, ServiceIds = serviceIds };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync((ProfessionalProfession)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalProfessionNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithInvalidServiceId_ShouldThrowException()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var command = new AddMultipleProfessionalServicesCommand { ProfessionalProfessionId = professionalProfessionId, ServiceIds = new List<Guid> { serviceId } };

        var professionalProfession = new ProfessionalProfession(professionalProfessionId, professionalId);
        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId, CancellationToken.None))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Service)null);

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithDuplicateService_ShouldThrowException()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var command = new AddMultipleProfessionalServicesCommand { ProfessionalProfessionId = professionalProfessionId, ServiceIds = new List<Guid> { serviceId } };

        var professionalProfession = new ProfessionalProfession(professionalProfessionId, professionalId);
        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId, CancellationToken.None))
            .ReturnsAsync(new MSProfessionals.Domain.Entities.Service { Id = serviceId });

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAndServiceIdAsync(professionalProfessionId, serviceId, CancellationToken.None))
            .ReturnsAsync(new MSProfessionals.Domain.Entities.ProfessionalService());

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateServiceException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithServiceLimitExceeded_ShouldThrowException()
    {
        // Arrange
        var professionalProfessionId = Guid.NewGuid();
        var professionalId = Guid.NewGuid();
        var serviceIds = new List<Guid> { Guid.NewGuid() };
        var command = new AddMultipleProfessionalServicesCommand { ProfessionalProfessionId = professionalProfessionId, ServiceIds = serviceIds };

        var professionalProfession = new ProfessionalProfession(professionalProfessionId, professionalId);
        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(professionalProfession);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, CancellationToken.None))
            .ReturnsAsync(new List<ProfessionalProfession> { professionalProfession });

        var existingServices = new List<MSProfessionals.Domain.Entities.ProfessionalService>();
        for (int i = 0; i < 10; i++)
        {
            existingServices.Add(new MSProfessionals.Domain.Entities.ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = professionalProfessionId,
                ServiceId = Guid.NewGuid(),
                Service = new MSProfessionals.Domain.Entities.Service { Id = Guid.NewGuid() }
            });
        }

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAsync(professionalProfessionId, CancellationToken.None))
            .ReturnsAsync(existingServices);

        foreach (var serviceId in serviceIds)
        {
            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId, CancellationToken.None))
                .ReturnsAsync(new MSProfessionals.Domain.Entities.Service { Id = serviceId });
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProfessionalServiceLimitExceededException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Adding these services would exceed the limit of 10 services per profession", exception.Message);
    }
} 