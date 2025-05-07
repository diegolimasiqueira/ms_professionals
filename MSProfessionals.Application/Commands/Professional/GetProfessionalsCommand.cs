using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to get paginated professionals
/// </summary>
public class GetProfessionalsCommand : IRequest<GetProfessionalsCommandResponse>
{
    /// <summary>
    /// Page number (1-based)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
} 