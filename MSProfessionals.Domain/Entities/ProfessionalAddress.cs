namespace MSProfessionals.Domain.Entities
{
    public class ProfessionalAddress
    {
        public Guid Id { get; set; }
        public Guid ProfessionalId { get; set; }
        public Professional Professional { get; set; } = null!;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDefault { get; set; }
        public Guid CountryId { get; set; }
        public CountryCode Country { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 