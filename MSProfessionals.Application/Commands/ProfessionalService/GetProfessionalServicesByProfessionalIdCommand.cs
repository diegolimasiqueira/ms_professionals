using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to get professional services by professional ID
/// </summary>
public class GetProfessionalServicesByProfessionalIdCommand : IRequest<IEnumerable<GetProfessionalServiceByIdCommandResponse>>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid ProfessionalId { get; set; }
} 