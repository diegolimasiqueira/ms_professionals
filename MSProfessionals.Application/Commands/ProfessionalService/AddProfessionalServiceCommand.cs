using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to add a professional service
/// </summary>
public class AddProfessionalServiceCommand : IRequest<AddProfessionalServiceCommandResponse>
{
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