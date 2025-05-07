using Moq;
using MSProfessionals.Application.Commands.ProfessionalService;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.UnitTests.Handlers.ProfessionalService;

public class DeleteProfessionalServiceCommandHandlerTests
{
    private readonly Mock<IProfessionalServiceRepository> _professionalServiceRepositoryMock;
    private readonly DeleteProfessionalServiceCommandHandler _handler;

    public DeleteProfessionalServiceCommandHandlerTests()
    {
        _professionalServiceRepositoryMock = new Mock<IProfessionalServiceRepository>();
        _handler = new DeleteProfessionalServiceCommandHandler(_professionalServiceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldDeleteProfessionalService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteProfessionalServiceCommand { Id = id };

        var professionalService = new MSProfessionals.Domain.Entities.ProfessionalService { Id = id };
        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(id, CancellationToken.None))
            .ReturnsAsync(professionalService);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _professionalServiceRepositoryMock.Verify(x => x.DeleteAsync(professionalService, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteProfessionalServiceCommand { Id = id };

        _professionalServiceRepositoryMock.Setup(x => x.GetByIdAsync(id, CancellationToken.None))
            .ReturnsAsync((MSProfessionals.Domain.Entities.ProfessionalService)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalServiceNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
} 