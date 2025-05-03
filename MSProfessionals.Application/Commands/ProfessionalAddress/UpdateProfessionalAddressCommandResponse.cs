using System;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Response for the UpdateProfessionalAddressCommand
/// </summary>
public class UpdateProfessionalAddressCommandResponse
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
    /// Latitude
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Whether this is the default address
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Country ID
    /// </summary>
    public Guid CountryId { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the UpdateProfessionalAddressCommandResponse
    /// </summary>
    /// <param name="professionalAddress">Professional address entity</param>
    public UpdateProfessionalAddressCommandResponse(Domain.Entities.ProfessionalAddress professionalAddress)
    {
        Id = professionalAddress.Id;
        ProfessionalId = professionalAddress.ProfessionalId;
        StreetAddress = professionalAddress.StreetAddress;
        City = professionalAddress.City;
        State = professionalAddress.State;
        PostalCode = professionalAddress.PostalCode;
        Latitude = professionalAddress.Latitude;
        Longitude = professionalAddress.Longitude;
        IsDefault = professionalAddress.IsDefault;
        CountryId = professionalAddress.CountryId;
        CreatedAt = professionalAddress.CreatedAt;
        UpdatedAt = professionalAddress.UpdatedAt;
    }
} 