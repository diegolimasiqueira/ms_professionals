using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the GetProfessionalByNameCommand
/// </summary>
public class GetProfessionalByNameCommandHandler : IRequestHandler<GetProfessionalByNameCommand, GetProfessionalByNameCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalByNameCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalByNameCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the retrieval of a professional by name
    /// </summary>
    /// <param name="request">Get professional by name command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional</returns>
    public async Task<GetProfessionalByNameCommandResponse> Handle(GetProfessionalByNameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Professional name cannot be empty", nameof(request.Name));
            }

            // Get professional
            var professional = await _professionalRepository.GetByEmailAsync(request.Name, cancellationToken);
            if (professional == null)
            {
                throw new ProfessionalNotFoundException($"Professional with name '{request.Name}' not found");
            }

            // Return response
            return new GetProfessionalByNameCommandResponse(professional);
        }
        catch (ProfessionalNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while retrieving the professional", ex);
        }
    }
} 