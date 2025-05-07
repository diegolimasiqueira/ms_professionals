using MSProfessionals.Domain.Entities;
using Xunit;
using TimeZone = MSProfessionals.Domain.Entities.TimeZone;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class TimeZoneTests
    {
        [Fact]
        public void TimeZone_ShouldSetAndGetProperties()
        {
            // Arrange
            var timeZone = new TimeZone
            {
                Id = Guid.NewGuid(),
                Name = "America/Sao_Paulo",
                Description = "Horário de Brasília"
            };

            // Assert
            Assert.NotEqual(Guid.Empty, timeZone.Id);
            Assert.Equal("America/Sao_Paulo", timeZone.Name);
            Assert.Equal("Horário de Brasília", timeZone.Description);
        }

        [Fact]
        public void TimeZone_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var timeZone = new TimeZone();

            // Assert
            Assert.Equal(string.Empty, timeZone.Name);
            Assert.Equal(string.Empty, timeZone.Description);
        }
    }
} 