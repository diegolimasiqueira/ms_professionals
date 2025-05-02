using System.ComponentModel.DataAnnotations;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the GetProfessionalByNameCommand
/// </summary>
public class GetProfessionalByNameCommandHandler : IRequestHandler<GetProfessionalByNameCommand, Domain.Entities.Professional>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalByNameCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalByNameCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the retrieval of a professional by name
    /// </summary>
    /// <param name="request">Get professional by name command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional</returns>
    public async Task<Domain.Entities.Professional> Handle(GetProfessionalByNameCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

         var professional = await _professionalRepository.GetAsync(
        p => EF.Functions.Like(p.Name.ToLower(), $"%{request.Name.ToLower()}%"),
        cancellationToken);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with name {request.Name} not found");
        }

        return professional;
    }
} 