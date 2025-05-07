using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to update a professional service
/// </summary>
public class UpdateProfessionalServiceCommand : IRequest<UpdateProfessionalServiceCommandResponse>
{
    /// <summary>
    /// Professional service ID
    /// </summary>
    [Required(ErrorMessage = "Professional service ID is required")]
    public Guid Id { get; set; }

    /// <summary>
    /// Professional profession ID
    /// </summary>
    [Required(ErrorMessage = "Professional profession ID is required")]
    public Guid ProfessionalProfessionId { get; set; }

    /// <summary>
    /// Service ID
    /// </summary>
    [Required(ErrorMessage = "Service ID is required")]
    public Guid ServiceId { get; set; }
} 