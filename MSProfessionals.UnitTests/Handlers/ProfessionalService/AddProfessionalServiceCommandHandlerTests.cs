using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class AddProfessionalServiceCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly AddProfessionalServiceCommandHandler _handler;

    public AddProfessionalServiceCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _professionalProfessionRepositoryMock = new Mock<IProfessionalProfessionRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _handler = new AddProfessionalServiceCommandHandler(
            _professionalServiceRepositoryMock.Object,
            _professionalProfessionRepositoryMock.Object,
            _serviceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddService_WhenValidCommand()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = serviceId
        };

        var professionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
        {
            Id = command.ProfessionalProfessionId,
            Profession = new MSProfessionals.Domain.Entities.Profession
            {
                Id = professionId,
                Name = "Test Profession"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var service = new MSProfessionals.Domain.Entities.Service
        {
            Id = serviceId,
            Name = "Test Service"
        };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAndServiceIdAsync(
            command.ProfessionalProfessionId,
            command.ServiceId,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalService)null!);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalProfession> { professionalProfession });

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAsync(professionalProfession.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalService>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.ProfessionalProfessionId, result.ProfessionalProfessionId);
        Assert.Equal(command.ServiceId, result.ServiceId);

        _professionalServiceRepositoryMock.Verify(x => x.AddAsync(It.Is<MSProfessionals.Domain.Entities.ProfessionalService>(s =>
            s.ProfessionalProfessionId == command.ProfessionalProfessionId &&
            s.ServiceId == command.ServiceId &&
            s.CreatedAt != DateTime.MinValue &&
            s.UpdatedAt != DateTime.MinValue
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalProfessionNotFoundException_WhenProfessionalProfessionDoesNotExist()
    {
        // Arrange
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalProfession)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalProfessionNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowServiceNotFoundException_WhenServiceDoesNotExist()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid()
        };

        var professionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
        {
            Id = command.ProfessionalProfessionId,
            Profession = new MSProfessionals.Domain.Entities.Profession
            {
                Id = professionId,
                Name = "Test Profession"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.Service)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowDuplicateServiceException_WhenServiceAlreadyExists()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = serviceId
        };

        var professionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
        {
            Id = command.ProfessionalProfessionId,
            Profession = new MSProfessionals.Domain.Entities.Profession
            {
                Id = professionId,
                Name = "Test Profession"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var service = new MSProfessionals.Domain.Entities.Service
        {
            Id = serviceId,
            Name = "Test Service"
        };

        var existingService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = command.ProfessionalProfessionId,
            ServiceId = command.ServiceId,
            Service = service,
            ProfessionalProfession = professionalProfession,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAndServiceIdAsync(
            command.ProfessionalProfessionId,
            command.ServiceId,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingService);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateServiceException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowProfessionalServiceLimitExceededException_WhenServiceLimitExceeded()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var command = new AddProfessionalServiceCommand
        {
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = serviceId
        };

        var professionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(professionalId, professionId)
        {
            Id = command.ProfessionalProfessionId,
            Profession = new MSProfessionals.Domain.Entities.Profession
            {
                Id = professionId,
                Name = "Test Profession"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var service = new MSProfessionals.Domain.Entities.Service
        {
            Id = serviceId,
            Name = "Test Service"
        };

        var existingServices = new List<MSProfessionals.Domain.Entities.ProfessionalService>();
        for (int i = 0; i < 10; i++)
        {
            existingServices.Add(new MSProfessionals.Domain.Entities.ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = professionalProfession.Id,
                ServiceId = Guid.NewGuid(),
                Service = new MSProfessionals.Domain.Entities.Service
                {
                    Id = Guid.NewGuid(),
                    Name = $"Test Service {i}"
                },
                ProfessionalProfession = professionalProfession,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        _professionalProfessionRepositoryMock.Setup(x => x.GetByIdAsync(command.ProfessionalProfessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professionalProfession);

        _serviceRepositoryMock.Setup(x => x.GetByIdAsync(command.ServiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAndServiceIdAsync(
            command.ProfessionalProfessionId,
            command.ServiceId,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalService)null!);

        _professionalProfessionRepositoryMock.Setup(x => x.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.ProfessionalProfession> { professionalProfession });

        _professionalServiceRepositoryMock.Setup(x => x.GetByProfessionalProfessionIdAsync(professionalProfession.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingServices);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalServiceLimitExceededException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 