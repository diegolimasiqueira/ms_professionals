using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class LanguageTests
    {
        [Fact]
        public void Language_ShouldSetAndGetProperties()
        {
            // Arrange
            var language = new Language
            {
                Id = Guid.NewGuid(),
                Code = "pt-BR",
                Description = "Português (Brasil)"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, language.Id);
            Assert.Equal("pt-BR", language.Code);
            Assert.Equal("Português (Brasil)", language.Description);
        }

        [Fact]
        public void Language_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var language = new Language();

            // Assert
            Assert.Equal(string.Empty, language.Code);
            Assert.Equal(string.Empty, language.Description);
        }
    }
} 