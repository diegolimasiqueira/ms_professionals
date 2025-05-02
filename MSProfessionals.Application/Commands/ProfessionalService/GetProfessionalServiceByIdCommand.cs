using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Command to get a professional service by ID
/// </summary>
public class GetProfessionalServiceByIdCommand : IRequest<GetProfessionalServiceByIdCommandResponse>
{
    /// <summary>
    /// Professional service ID
    /// </summary>
    [Required(ErrorMessage = "Professional service ID is required")]
    public Guid Id { get; set; }
} 