using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to add multiple professional services at once
/// </summary>
public class AddMultipleProfessionalServicesCommand : IRequest<IEnumerable<AddProfessionalServiceCommandResponse>>
{
    /// <summary>
    /// Professional profession ID
    /// </summary>
    [Required(ErrorMessage = "Professional profession ID is required")]
    public Guid ProfessionalProfessionId { get; set; }

    /// <summary>
    /// List of service IDs to add
    /// </summary>
    [Required(ErrorMessage = "At least one service ID is required")]
    [MinLength(1, ErrorMessage = "At least one service ID is required")]
    public IEnumerable<Guid> ServiceIds { get; set; } = new List<Guid>();
} 