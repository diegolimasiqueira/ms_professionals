using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the UpdateProfessionalServiceCommand
/// </summary>
public class UpdateProfessionalServiceCommandHandler : IRequestHandler<UpdateProfessionalServiceCommand, UpdateProfessionalServiceCommandResponse>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateProfessionalServiceCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    /// <param name="serviceRepository">Service repository</param>
    public UpdateProfessionalServiceCommandHandler(
        IProfessionalServiceRepository professionalServiceRepository,
        IProfessionalProfessionRepository professionalProfessionRepository,
        IServiceRepository serviceRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
        _professionalProfessionRepository = professionalProfessionRepository;
        _serviceRepository = serviceRepository;
    }

    /// <summary>
    /// Handles the update of a professional service
    /// </summary>
    /// <param name="request">Update professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional service</returns>
    public async Task<UpdateProfessionalServiceCommandResponse> Handle(UpdateProfessionalServiceCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if the professional service exists
        var professionalService = await _professionalServiceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (professionalService == null)
        {
            throw new ProfessionalServiceNotFoundException($"Professional service with ID {request.Id} not found");
        }

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

        try
        {
            // Update the professional service
            professionalService.ProfessionalProfessionId = request.ProfessionalProfessionId;
            professionalService.ServiceId = request.ServiceId;
            professionalService.UpdatedAt = DateTime.UtcNow;

            // Save the changes
            await _professionalServiceRepository.UpdateAsync(professionalService, cancellationToken);

            return new UpdateProfessionalServiceCommandResponse(professionalService);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                switch (pgEx.SqlState)
                {
                    case "23503": // Foreign key violation
                        throw new InvalidOperationException($"Cannot update professional service with ID {request.Id} because the referenced entity does not exist");
                    case "23505": // Unique constraint violation
                        throw new InvalidOperationException($"Cannot update professional service with ID {request.Id} because it violates a unique constraint");
                    default:
                        throw;
                }
            }
            throw;
        }
    }
} 