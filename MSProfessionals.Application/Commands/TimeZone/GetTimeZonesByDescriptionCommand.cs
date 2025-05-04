using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.TimeZone;

/// <summary>
/// Command to get time zones filtered by description
/// </summary>
public class GetTimeZonesByDescriptionCommand : IRequest<GetTimeZonesCommandResponse>
{
    /// <summary>
    /// Gets or sets the description to filter by
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    [StringLength(100, ErrorMessage = "Description must be at most 100 characters")]
    public string Description { get; set; } = string.Empty;

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