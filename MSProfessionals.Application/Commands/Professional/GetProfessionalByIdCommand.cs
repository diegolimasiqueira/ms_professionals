using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to get a professional by ID
/// </summary>
public class GetProfessionalByIdCommand : IRequest<Domain.Entities.Professional>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid Id { get; set; }
} 