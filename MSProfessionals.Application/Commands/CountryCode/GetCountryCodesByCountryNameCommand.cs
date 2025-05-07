using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.CountryCode;

/// <summary>
/// Command to get country codes filtered by country name
/// </summary>
public class GetCountryCodesByCountryNameCommand : IRequest<GetCountryCodesCommandResponse>
{
    /// <summary>
    /// Gets or sets the country name to filter by
    /// </summary>
    [Required]
    public string CountryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page number
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
} 