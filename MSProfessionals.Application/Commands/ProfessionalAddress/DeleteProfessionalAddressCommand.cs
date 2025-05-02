using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command para deletar um endereço
/// </summary>
public class DeleteProfessionalAddressCommand : IRequest<Unit>
{
    /// <summary>
    /// ID do endereço
    /// </summary>
    [Required]
    public Guid Id { get; set; }
} 