using MSProfessionals.Domain.Entities;

namespace MSProfessionals.UnitTests.Domain.Entities
{
    public class ProfessionalTests
    {
        [Fact]
        public void Professional_ShouldInitializeCollections()
        {
            // Arrange & Act
            var professional = new Professional();

            // Assert
            Assert.NotNull(professional.Addresses);
            Assert.NotNull(professional.ProfessionalProfessions);
            Assert.Empty(professional.Addresses);
            Assert.Empty(professional.ProfessionalProfessions);
        }

        [Fact]
        public void Professional_ShouldInitializeStringProperties()
        {
            // Arrange & Act
            var professional = new Professional();

            // Assert
            Assert.Equal(string.Empty, professional.Name);
            Assert.Equal(string.Empty, professional.DocumentId);
            Assert.Equal(string.Empty, professional.PhoneNumber);
            Assert.Equal(string.Empty, professional.Email);
            Assert.Null(professional.PhotoUrl);
            Assert.Null(professional.SocialMedia);
            Assert.Null(professional.Media);
        }

        [Fact]
        public void Professional_ShouldSetAndGetProperties()
        {
            // Arrange
            var professional = new Professional
            {
                Id = Guid.NewGuid(),
                Name = "João Silva",
                DocumentId = "123.456.789-00",
                PhotoUrl = "https://example.com/photo.jpg",
                PhoneNumber = "+5511999999999",
                Email = "joao.silva@example.com",
                SocialMedia = new Dictionary<string, string>
                {
                    { "linkedin", "https://linkedin.com/in/joaosilva" },
                    { "instagram", "@joaosilva" }
                },
                Media = new Dictionary<string, string>
                {
                    { "portfolio", "https://joaosilva.com" }
                },
                CurrencyId = Guid.NewGuid(),
                PhoneCountryCodeId = Guid.NewGuid(),
                PreferredLanguageId = Guid.NewGuid(),
                TimezoneId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.NotEqual(Guid.Empty, professional.Id);
            Assert.Equal("João Silva", professional.Name);
            Assert.Equal("123.456.789-00", professional.DocumentId);
            Assert.Equal("https://example.com/photo.jpg", professional.PhotoUrl);
            Assert.Equal("+5511999999999", professional.PhoneNumber);
            Assert.Equal("joao.silva@example.com", professional.Email);
            Assert.NotNull(professional.SocialMedia);
            Assert.Equal(2, professional.SocialMedia.Count);
            Assert.NotNull(professional.Media);
            Assert.Single(professional.Media);
            Assert.NotEqual(Guid.Empty, professional.CurrencyId);
            Assert.NotEqual(Guid.Empty, professional.PhoneCountryCodeId);
            Assert.NotEqual(Guid.Empty, professional.PreferredLanguageId);
            Assert.NotEqual(Guid.Empty, professional.TimezoneId);
            Assert.True(professional.CreatedAt <= DateTime.UtcNow);
            Assert.True(professional.UpdatedAt <= DateTime.UtcNow);
        }
    }
} 