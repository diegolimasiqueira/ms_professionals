namespace MSProfessionals.Domain.Entities
{
    public class CountryCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
    }
} 