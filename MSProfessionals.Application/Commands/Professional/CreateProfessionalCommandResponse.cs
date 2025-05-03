using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.Professional
{
    public class CreateProfessionalCommandResponse
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
        public Guid PhoneCountryCodeId { get; set; }        
        public Guid PreferredLanguageId { get; set; }
        public Guid TimezoneId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 