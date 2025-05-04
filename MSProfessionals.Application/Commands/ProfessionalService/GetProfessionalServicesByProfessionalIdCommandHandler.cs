using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the GetProfessionalServicesByProfessionalIdCommand
/// </summary>
public class GetProfessionalServicesByProfessionalIdCommandHandler : IRequestHandler<GetProfessionalServicesByProfessionalIdCommand, IEnumerable<GetProfessionalServiceByIdCommandResponse>>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalServicesByProfessionalIdCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalServicesByProfessionalIdCommandHandler(
        IProfessionalServiceRepository professionalServiceRepository,
        IProfessionalRepository professionalRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the retrieval of professional services by professional ID
    /// </summary>
    /// <param name="request">Get professional services command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional services</returns>
    /// <exception cref="ProfessionalNotFoundException">Thrown when the professional is not found</exception>
    public async Task<IEnumerable<GetProfessionalServiceByIdCommandResponse>> Handle(
        GetProfessionalServicesByProfessionalIdCommand request,
        CancellationToken cancellationToken)
    {
        // Check if the professional exists and load related entities
        var professional = await _professionalRepository.GetAsync(p => p.Id == request.ProfessionalId, cancellationToken);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Get all services for the professional
        var professionalProfessions = professional.ProfessionalProfessions;
        var services = new List<Domain.Entities.ProfessionalService>();

        foreach (var profession in professionalProfessions)
        {
            var professionServices = await _professionalServiceRepository.GetByProfessionalProfessionIdAsync(profession.Id, cancellationToken);
            services.AddRange(professionServices);
        }

        // Map to response
        return services.Select(service => new GetProfessionalServiceByIdCommandResponse(service));
    }
} 