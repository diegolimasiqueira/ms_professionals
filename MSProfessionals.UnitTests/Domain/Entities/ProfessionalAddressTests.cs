using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ProfessionalAddressTests
    {
        [Fact]
        public void Constructor_ShouldCreateProfessionalAddressWithCorrectValues()
        {
            // Arrange
            var professionalId = Guid.NewGuid();
            var streetAddress = "Rua Teste, 123";
            var city = "São Paulo";
            var state = "SP";
            var postalCode = "01234-567";
            var latitude = -23.5505;
            var longitude = -46.6333;
            var isDefault = true;
            var countryId = Guid.NewGuid();

            // Act
            var address = new ProfessionalAddress(
                professionalId,
                streetAddress,
                city,
                state,
                postalCode,
                latitude,
                longitude,
                isDefault,
                countryId);

            // Assert
            Assert.NotEqual(Guid.Empty, address.Id);
            Assert.Equal(professionalId, address.ProfessionalId);
            Assert.Equal(streetAddress, address.StreetAddress);
            Assert.Equal(city, address.City);
            Assert.Equal(state, address.State);
            Assert.Equal(postalCode, address.PostalCode);
            Assert.Equal(latitude, address.Latitude);
            Assert.Equal(longitude, address.Longitude);
            Assert.Equal(isDefault, address.IsDefault);
            Assert.Equal(countryId, address.CountryId);
            Assert.True(address.CreatedAt <= DateTime.UtcNow);
            Assert.True(address.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Constructor_ShouldCreateProfessionalAddressWithNullCoordinates()
        {
            // Arrange
            var professionalId = Guid.NewGuid();
            var streetAddress = "Rua Teste, 123";
            var city = "São Paulo";
            var state = "SP";
            var postalCode = "01234-567";
            double? latitude = null;
            double? longitude = null;
            var isDefault = true;
            var countryId = Guid.NewGuid();

            // Act
            var address = new ProfessionalAddress(
                professionalId,
                streetAddress,
                city,
                state,
                postalCode,
                latitude,
                longitude,
                isDefault,
                countryId);

            // Assert
            Assert.Null(address.Latitude);
            Assert.Null(address.Longitude);
        }
    }
} 