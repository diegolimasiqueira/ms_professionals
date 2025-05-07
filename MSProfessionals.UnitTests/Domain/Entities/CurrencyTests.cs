using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class CurrencyTests
    {
        [Fact]
        public void Currency_ShouldSetAndGetProperties()
        {
            // Arrange
            var currency = new Currency
            {
                Id = Guid.NewGuid(),
                Code = "BRL",
                Description = "Real Brasileiro"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, currency.Id);
            Assert.Equal("BRL", currency.Code);
            Assert.Equal("Real Brasileiro", currency.Description);
        }

        [Fact]
        public void Currency_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var currency = new Currency();

            // Assert
            Assert.Equal(string.Empty, currency.Code);
            Assert.Equal(string.Empty, currency.Description);
        }
    }
} 