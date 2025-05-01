namespace MSProfessionals.Domain.Entities
{
    public class Professional
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Dictionary<string, string>? SocialMedia { get; set; }
        public Dictionary<string, string>? Media { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;
        public Guid PhoneCountryCodeId { get; set; }
        public CountryCode PhoneCountryCode { get; set; } = null!;
        public Guid PreferredLanguageId { get; set; }
        public Language PreferredLanguage { get; set; } = null!;
        public Guid TimezoneId { get; set; }
        public TimeZone Timezone { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ProfessionalAddress> Addresses { get; set; } = new List<ProfessionalAddress>();
    }
} 