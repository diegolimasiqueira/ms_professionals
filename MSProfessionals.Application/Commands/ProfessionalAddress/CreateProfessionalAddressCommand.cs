using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command to create a professional address
/// </summary>
public class CreateProfessionalAddressCommand : IRequest<CreateProfessionalAddressCommandResponse>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required]
    public Guid ProfessionalId { get; set; }

    /// <summary>
    /// Street address
    /// </summary>
    [Required]
    [StringLength(200)]
    public string StreetAddress { get; set; } = string.Empty;

    /// <summary>
    /// City
    /// </summary>
    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State
    /// </summary>
    [Required]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Postal code
    /// </summary>
    [Required]
    [StringLength(20)]
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
    [Required]
    public Guid CountryId { get; set; }
} 