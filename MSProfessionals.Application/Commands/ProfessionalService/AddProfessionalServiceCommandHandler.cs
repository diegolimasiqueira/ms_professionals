using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the AddProfessionalServiceCommand
/// </summary>
public class AddProfessionalServiceCommandHandler : IRequestHandler<AddProfessionalServiceCommand, AddProfessionalServiceCommandResponse>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the AddProfessionalServiceCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    /// <param name="serviceRepository">Service repository</param>
    public AddProfessionalServiceCommandHandler(
        IProfessionalServiceRepository professionalServiceRepository,
        IProfessionalProfessionRepository professionalProfessionRepository,
        IServiceRepository serviceRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
        _professionalProfessionRepository = professionalProfessionRepository;
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Handles the addition of a professional service
    /// </summary>
    /// <param name="request">Add professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The added professional service</returns>
    public async Task<AddProfessionalServiceCommandResponse> Handle(AddProfessionalServiceCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if the professional profession exists
        var professionalProfession = await _professionalProfessionRepository.GetByIdAsync(request.ProfessionalProfessionId, cancellationToken);
        if (professionalProfession == null)
        {
            throw new ProfessionalProfessionNotFoundException($"Professional profession with ID {request.ProfessionalProfessionId} not found");
        }

        // Check if the service exists
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken);
        if (service == null)
        {
            throw new ServiceNotFoundException($"Service with ID {request.ServiceId} not found");
        }

        // Check if the service is already associated with this professional profession
        var existingService = await _professionalServiceRepository.GetByProfessionalProfessionIdAndServiceIdAsync(
            request.ProfessionalProfessionId, 
            request.ServiceId, 
            cancellationToken);
        
        if (existingService != null)
        {
            throw new DuplicateServiceException($"Service with ID {request.ServiceId} is already associated with this professional profession");
        }

        // Get current services for this profession
        var currentServices = await _professionalServiceRepository.GetByProfessionalProfessionIdAsync(request.ProfessionalProfessionId, cancellationToken);

        // Check if professional already has 10 services for this profession
        if (currentServices.Count() >= 10)
        {
            throw new ProfessionalServiceLimitExceededException("Professional cannot have more than 10 services per profession");
        }

        // Create the professional service
        var professionalService = new Domain.Entities.ProfessionalService
        {
            Id = Guid.NewGuid(),
            ProfessionalProfessionId = request.ProfessionalProfessionId,
            ServiceId = request.ServiceId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add the professional service
        await _professionalServiceRepository.AddAsync(professionalService, cancellationToken);

        return new AddProfessionalServiceCommandResponse(professionalService);
    }
} 