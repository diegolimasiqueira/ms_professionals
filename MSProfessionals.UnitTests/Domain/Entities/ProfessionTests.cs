using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ProfessionTests
    {
        [Fact]
        public void Profession_ShouldSetAndGetProperties()
        {
            // Arrange
            var profession = new Profession
            {
                Id = Guid.NewGuid(),
                Name = "Médico"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, profession.Id);
            Assert.Equal("Médico", profession.Name);
        }

        [Fact]
        public void Profession_ShouldInitializeCollections()
        {
            // Arrange & Act
            var profession = new Profession();

            // Assert
            Assert.NotNull(profession.ProfessionalProfessions);
            Assert.Empty(profession.ProfessionalProfessions);
        }

        [Fact]
        public void Profession_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var profession = new Profession();

            // Assert
            Assert.Equal(string.Empty, profession.Name);
        }
    }
} 