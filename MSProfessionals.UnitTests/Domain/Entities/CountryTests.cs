using MSProfessionals.Domain.Entities;
using Xunit;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class CountryTests
    {
        [Fact]
        public void Country_ShouldSetAndGetProperties()
        {
            // Arrange
            var country = new Country
            {
                Id = Guid.NewGuid(),
                Name = "Brasil",
                Code = "BR",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.NotEqual(Guid.Empty, country.Id);
            Assert.Equal("Brasil", country.Name);
            Assert.Equal("BR", country.Code);
            Assert.True(country.CreatedAt <= DateTime.UtcNow);
            Assert.True(country.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Country_ShouldInitializeCollections()
        {
            // Arrange & Act
            var country = new Country();

            // Assert
            Assert.NotNull(country.ProfessionalAddresses);
            Assert.Empty(country.ProfessionalAddresses);
        }

        [Fact]
        public void Country_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var country = new Country();

            // Assert
            Assert.Equal(string.Empty, country.Name);
            Assert.Equal(string.Empty, country.Code);
        }

        [Fact]
        public void Country_ShouldAllowNullUpdatedAt()
        {
            // Arrange
            var country = new Country
            {
                Id = Guid.NewGuid(),
                Name = "Brasil",
                Code = "BR",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Assert
            Assert.Null(country.UpdatedAt);
        }
    }
} 