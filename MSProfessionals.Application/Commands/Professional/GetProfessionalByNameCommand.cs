using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Command to get a professional by name
/// </summary>
public class GetProfessionalByNameCommand : IRequest<GetProfessionalByNameCommandResponse>
{
    /// <summary>
    /// Professional's name
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
} 