using Moq;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Application.Commands.Service;


namespace MSProfessionals.UnitTests.Handlers.Service
{
    public class GetServicesCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly GetServicesCommandHandler _handler;

        public GetServicesCommandHandlerTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _handler = new GetServicesCommandHandler(_serviceRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnServices_WhenServicesExist()
        {
            // Arrange
            var command = new GetServicesCommand { PageNumber = 1, PageSize = 10 };
            var skip = (command.PageNumber - 1) * command.PageSize;
            var services = new List<MSProfessionals.Domain.Entities.Service>
            {
                new() { Id = Guid.NewGuid(), Name = "Service 1" },
                new() { Id = Guid.NewGuid(), Name = "Service 2" }
            };

            _serviceRepositoryMock.Setup(x => x.GetAllAsync(skip, command.PageSize, command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(services);
            _serviceRepositoryMock.Setup(x => x.CountAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
            Assert.Contains(result.Items, s => s.Name == "Service 1");
            Assert.Contains(result.Items, s => s.Name == "Service 2");
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoServicesExist()
        {
            // Arrange
            var command = new GetServicesCommand { PageNumber = 1, PageSize = 10 };
            var skip = (command.PageNumber - 1) * command.PageSize;

            _serviceRepositoryMock.Setup(x => x.GetAllAsync(skip, command.PageSize, command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MSProfessionals.Domain.Entities.Service>());
            _serviceRepositoryMock.Setup(x => x.CountAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(0, result.TotalPages);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _handler.Handle(null!, CancellationToken.None));
        }
    }
} 