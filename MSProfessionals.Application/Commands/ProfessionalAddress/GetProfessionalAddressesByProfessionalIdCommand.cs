using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command to get professional addresses by professional ID
/// </summary>
public class GetProfessionalAddressesByProfessionalIdCommand : IRequest<IEnumerable<GetProfessionalAddressByIdCommandResponse>>
{
    /// <summary>
    /// Professional ID
    /// </summary>
    [Required(ErrorMessage = "Professional ID is required")]
    public Guid ProfessionalId { get; set; }
} 