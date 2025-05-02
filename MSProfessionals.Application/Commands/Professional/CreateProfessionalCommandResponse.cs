using System;

namespace MSProfessionals.Application.Commands.Professional
{
    public class CreateProfessionalCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid CurrencyId { get; set; }
        public Guid PhoneCountryCodeId { get; set; }        
        public Guid PreferredLanguageId { get; set; }
        public Guid TimezoneId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 