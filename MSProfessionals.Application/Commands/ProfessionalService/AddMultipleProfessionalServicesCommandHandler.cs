using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the AddMultipleProfessionalServicesCommand
/// </summary>
public class AddMultipleProfessionalServicesCommandHandler : IRequestHandler<AddMultipleProfessionalServicesCommand, IEnumerable<AddProfessionalServiceCommandResponse>>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the AddMultipleProfessionalServicesCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    /// <param name="serviceRepository">Service repository</param>
    public AddMultipleProfessionalServicesCommandHandler(
        IProfessionalServiceRepository professionalServiceRepository,
        IProfessionalProfessionRepository professionalProfessionRepository,
        IServiceRepository serviceRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
        _professionalProfessionRepository = professionalProfessionRepository;
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Handles the addition of multiple professional services
    /// </summary>
    /// <param name="request">Add multiple professional services command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of added professional services</returns>
    public async Task<IEnumerable<AddProfessionalServiceCommandResponse>> Handle(AddMultipleProfessionalServicesCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if the professional profession exists
        var professionalProfession = await _professionalProfessionRepository.GetByIdAsync(request.ProfessionalProfessionId, cancellationToken);
        if (professionalProfession == null)
        {
            throw new ProfessionalProfessionNotFoundException($"Professional profession with ID {request.ProfessionalProfessionId} not found");
        }

        // Get all professional professions for this professional
        var professionalProfessions = await _professionalProfessionRepository.GetByProfessionalIdAsync(professionalProfession.ProfessionalId, cancellationToken);
        
        // Count total services across all professions
        int totalServices = 0;
        foreach (var pp in professionalProfessions)
        {
            var services = await _professionalServiceRepository.GetByProfessionalProfessionIdAsync(pp.Id, cancellationToken);
            totalServices += services.Count();
        }

        // Check if adding new services would exceed the limit
        if (totalServices + request.ServiceIds.Count() > 10)
        {
            throw new ProfessionalServiceLimitExceededException("Adding these services would exceed the limit of 10 services per professional");
        }

        var responses = new List<AddProfessionalServiceCommandResponse>();

        foreach (var serviceId in request.ServiceIds)
        {
            // Check if the service exists
            var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (service == null)
            {
                throw new ServiceNotFoundException($"Service with ID {serviceId} not found");
            }

            // Check if the service is already associated with this professional profession
            var existingService = await _professionalServiceRepository.GetByProfessionalProfessionIdAndServiceIdAsync(
                request.ProfessionalProfessionId, 
                serviceId, 
                cancellationToken);
            
            if (existingService != null)
            {
                throw new DuplicateServiceException($"Service with ID {serviceId} is already associated with this professional profession");
            }

            // Create the professional service
            var professionalService = new Domain.Entities.ProfessionalService
            {
                Id = Guid.NewGuid(),
                ProfessionalProfessionId = request.ProfessionalProfessionId,
                ServiceId = serviceId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add the professional service
            await _professionalServiceRepository.AddAsync(professionalService, cancellationToken);

            responses.Add(new AddProfessionalServiceCommandResponse(professionalService));
        }

        return responses;
    }
} 