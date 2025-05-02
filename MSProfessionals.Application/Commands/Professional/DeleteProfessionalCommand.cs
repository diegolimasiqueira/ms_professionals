using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to delete a professional
/// </summary>
public class DeleteProfessionalCommand : IRequest<bool>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid Id { get; set; }
} 