using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ServiceTests
    {
        [Fact]
        public void Service_ShouldSetAndGetProperties()
        {
            // Arrange
            var service = new Service
            {
                Id = Guid.NewGuid(),
                Name = "Consulta Clínica"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, service.Id);
            Assert.Equal("Consulta Clínica", service.Name);
        }

        [Fact]
        public void Service_ShouldInitializeCollections()
        {
            // Arrange & Act
            var service = new Service();

            // Assert
            Assert.NotNull(service.ProfessionalServices);
            Assert.Empty(service.ProfessionalServices);
        }

        [Fact]
        public void Service_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var service = new Service();

            // Assert
            Assert.Equal(string.Empty, service.Name);
        }
    }
} 