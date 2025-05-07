using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.Profession;

/// <summary>
/// Command to add a profession to a professional
/// </summary>
public class AddProfessionCommand : IRequest<CreatedProfessionCommandResponse>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid ProfessionalId { get; set; }

    /// <summary>
    /// Profession name
    /// </summary>
    [Required(ErrorMessage = "Profession name is required")]
    [StringLength(50, ErrorMessage = "Profession name cannot exceed 50 characters")]
    public string ProfessionName { get; set; } = string.Empty;
} 