using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to create a professional
/// </summary>
public class CreateProfessionalCommand : IRequest<Domain.Entities.Professional>
{
    /// <summary>
    /// Professional's name
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Professional's document ID
    /// </summary>
    [Required(ErrorMessage = "Document ID is required")]
    [StringLength(20, ErrorMessage = "Document ID cannot exceed 20 characters")]
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Consumer's photo URL
    /// </summary>
    [StringLength(500, ErrorMessage = "Photo URL must have a maximum of 500 characters")]
    public string PhotoUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Professional's phone number
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [RegularExpression(@"^\+?[0-9\s-]+$", ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Professional's email
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Professional's social media links
    /// </summary>
    public Dictionary<string, string>? SocialMedia { get; set; }

    /// <summary>
    /// Professional's media links
    /// </summary>
    public Dictionary<string, string>? Media { get; set; }

    /// <summary>
    /// Professional's currency ID
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    public Guid CurrencyId { get; set; }

    /// <summary>
    /// Professional's phone country code ID
    /// </summary>
    [Required(ErrorMessage = "Phone country code ID is required")]
    public Guid PhoneCountryCodeId { get; set; }

    /// <summary>
    /// Professional's preferred language ID
    /// </summary>
    [Required(ErrorMessage = "Preferred language ID is required")]
    public Guid PreferredLanguageId { get; set; }

    /// <summary>
    /// Professional's timezone ID
    /// </summary>
    [Required(ErrorMessage = "Timezone ID is required")]
    public Guid TimezoneId { get; set; }
}