using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ProfessionalServiceTests
    {
        [Fact]
        public void ProfessionalService_ShouldSetAndGetProperties()
        {
            // Arrange
            var professionalService = new ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.NotEqual(Guid.Empty, professionalService.Id);
            Assert.NotEqual(Guid.Empty, professionalService.ProfessionalProfessionId);
            Assert.NotEqual(Guid.Empty, professionalService.ServiceId);
            Assert.True(professionalService.CreatedAt <= DateTime.UtcNow);
            Assert.True(professionalService.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void ProfessionalService_ShouldAllowNullUpdatedAt()
        {
            // Arrange
            var professionalService = new ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Assert
            Assert.Null(professionalService.UpdatedAt);
        }
    }
} 