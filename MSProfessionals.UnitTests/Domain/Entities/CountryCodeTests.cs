using MSProfessionals.Domain.Entities;
using Xunit;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class CountryCodeTests
    {
        [Fact]
        public void Constructor_ShouldCreateCountryCodeWithCorrectValues()
        {
            // Arrange
            var id = Guid.NewGuid();
            var code = "BR";
            var countryName = "Brasil";

            // Act
            var countryCode = new CountryCode(id, code, countryName);

            // Assert
            Assert.Equal(id, countryCode.Id);
            Assert.Equal(code, countryCode.Code);
            Assert.Equal(countryName, countryCode.CountryName);
            Assert.NotNull(countryCode.Professionals);
            Assert.Empty(countryCode.Professionals);
            Assert.NotNull(countryCode.Addresses);
            Assert.Empty(countryCode.Addresses);
        }

        [Fact]
        public void Update_ShouldUpdateCountryCodeValues()
        {
            // Arrange
            var countryCode = new CountryCode(Guid.NewGuid(), "BR", "Brasil");
            var newCode = "US";
            var newCountryName = "United States";

            // Act
            countryCode.Update(newCode, newCountryName);

            // Assert
            Assert.Equal(newCode, countryCode.Code);
            Assert.Equal(newCountryName, countryCode.CountryName);
        }
    }
} 