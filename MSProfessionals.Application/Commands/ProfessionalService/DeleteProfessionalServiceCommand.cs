using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to delete a professional service
/// </summary>
public class DeleteProfessionalServiceCommand : IRequest<Unit>
{
    /// <summary>
    /// Professional service ID
    /// </summary>
    [Required(ErrorMessage = "Professional service ID is required")]
    public Guid Id { get; set; }
} 