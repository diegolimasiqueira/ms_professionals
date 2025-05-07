using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command to update a professional address
/// </summary>
public class UpdateProfessionalAddressCommand : IRequest<UpdateProfessionalAddressCommandResponse>
{
    /// <summary>
    /// Professional address ID
    /// </summary>
    [Required(ErrorMessage = "Professional address ID is required")]
    public Guid Id { get; set; }

    /// <summary>
    /// Street address
    /// </summary>
    [Required(ErrorMessage = "Street address is required")]
    [StringLength(255, ErrorMessage = "Street address cannot exceed 255 characters")]
    public string StreetAddress { get; set; } = string.Empty;

    /// <summary>
    /// City
    /// </summary>
    [Required(ErrorMessage = "City is required")]
    [StringLength(30, ErrorMessage = "City cannot exceed 30 characters")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State
    /// </summary>
    [Required(ErrorMessage = "State is required")]
    [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Postal code
    /// </summary>
    [Required(ErrorMessage = "Postal code is required")]
    [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
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
    [Required(ErrorMessage = "Country ID is required")]
    public Guid CountryId { get; set; }
} 