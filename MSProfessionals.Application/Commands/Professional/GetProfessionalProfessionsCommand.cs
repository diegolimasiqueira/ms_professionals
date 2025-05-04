using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to get professional professions by professional ID
/// </summary>
public class GetProfessionalProfessionsCommand : IRequest<IEnumerable<GetProfessionalProfessionsCommandResponse>>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid ProfessionalId { get; set; }
} 