using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command to delete a professional address
/// </summary>
public class DeleteProfessionalAddressCommand : IRequest<Unit>
{
    /// <summary>
    /// Professional address ID
    /// </summary>
    [Required(ErrorMessage = "Professional address ID is required")]
    public Guid Id { get; set; }
} 