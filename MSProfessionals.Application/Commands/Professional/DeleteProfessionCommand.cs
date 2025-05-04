using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to delete a professional profession
/// </summary>
public class DeleteProfessionCommand : IRequest<Unit>
{
    /// <summary>
    /// Professional profession ID
    /// </summary>
    [Required(ErrorMessage = "Professional profession ID is required")]
    public Guid Id { get; set; }
} 