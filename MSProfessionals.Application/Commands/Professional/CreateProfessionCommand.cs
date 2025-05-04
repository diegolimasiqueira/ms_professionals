using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to create a profession for a professional
/// </summary>
public class CreateProfessionCommand : IRequest<CreateProfessionCommandResponse>
{
    /// <summary>
    /// Gets or sets the professional ID
    /// </summary>
    [Required]
    public Guid ProfessionalId { get; set; }

    /// <summary>
    /// Gets or sets the profession ID
    /// </summary>
    [Required]
    public Guid ProfessionId { get; set; }
} 