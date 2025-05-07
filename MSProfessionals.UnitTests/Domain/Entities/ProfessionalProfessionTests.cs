using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ProfessionalProfessionTests
    {
        [Fact]
        public void Constructor_ShouldCreateProfessionalProfessionWithCorrectValues()
        {
            // Arrange
            var professionalId = Guid.NewGuid();
            var professionId = Guid.NewGuid();

            // Act
            var professionalProfession = new ProfessionalProfession(professionalId, professionId);

            // Assert
            Assert.NotEqual(Guid.Empty, professionalProfession.Id);
            Assert.Equal(professionalId, professionalProfession.ProfessionalId);
            Assert.Equal(professionId, professionalProfession.ProfessionId);
            Assert.True(professionalProfession.CreatedAt <= DateTime.UtcNow);
            Assert.True(professionalProfession.UpdatedAt <= DateTime.UtcNow);
            Assert.NotNull(professionalProfession.ProfessionalServices);
            Assert.Empty(professionalProfession.ProfessionalServices);
        }

        [Fact]
        public void ProfessionalProfession_ShouldInitializeCollections()
        {
            // Arrange & Act
            var professionalProfession = new ProfessionalProfession(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.NotNull(professionalProfession.ProfessionalServices);
            Assert.Empty(professionalProfession.ProfessionalServices);
        }
    }
} 