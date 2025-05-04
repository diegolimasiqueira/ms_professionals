using System;

namespace MSProfessionals.Domain.Entities
{
    /// <summary>
    /// Professional address entity
    /// </summary>
    public class ProfessionalAddress
    {
        /// <summary>
        /// Professional address ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Professional ID
        /// </summary>
        public Guid ProfessionalId { get; set; }

        /// <summary>
        /// Professional navigation property
        /// </summary>
        public Professional Professional { get; set; } = null!;

        /// <summary>
        /// Street address
        /// </summary>
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Country ID
        /// </summary>
        public Guid CountryId { get; set; }
 
        /// <summary>
        /// Country navigation property
        /// </summary>
         public CountryCode? Countrycode { get; set; } = null!;

        /// <summary>
        /// Latitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Indicates if this is the default address
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProfessionalAddress
        /// </summary>
        public ProfessionalAddress(
            Guid professionalId,
            string streetAddress,
            string city,
            string state,
            string postalCode,
            double? latitude,
            double? longitude,
            bool isDefault,
            Guid countryId)
        {
            Id = Guid.NewGuid();
            ProfessionalId = professionalId;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
            IsDefault = isDefault;
            CountryId = countryId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 