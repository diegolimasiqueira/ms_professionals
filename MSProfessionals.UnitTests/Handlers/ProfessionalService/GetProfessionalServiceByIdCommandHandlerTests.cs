using Moq;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class GetProfessionalServiceByIdCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly GetProfessionalServiceByIdCommandHandler _handler;

    public GetProfessionalServiceByIdCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _handler = new GetProfessionalServiceByIdCommandHandler(_professionalServiceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldReturnProfessionalService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new GetProfessionalServiceByIdCommand { Id = id };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService
        {
            Id = id,
            ProfessionalProfessionId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Service = new MSProfessionals.Domain.Entities.Service { Name = "Test Service" },
            ProfessionalProfession = new MSProfessionals.Domain.Entities.ProfessionalProfession(Guid.NewGuid(), Guid.NewGuid())
            {
                Profession = new MSProfessionals.Domain.Entities.Profession { Name = "Test Profession" }
            }
        };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(id, CancellationToken.None))
            .ReturnsAsync(professionalService);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(professionalService.ProfessionalProfessionId, result.ProfessionalProfessionId);
        Assert.Equal(professionalService.ServiceId, result.ServiceId);
        Assert.Equal(professionalService.Service.Name, result.ServiceName);
        Assert.Equal(professionalService.ProfessionalProfession.Profession.Name, result.ProfessionName);
    }

    [Fact]
    public async Task Handle_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new GetProfessionalServiceByIdCommand { Id = id };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(id, CancellationToken.None))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalService)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalServiceNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
} 