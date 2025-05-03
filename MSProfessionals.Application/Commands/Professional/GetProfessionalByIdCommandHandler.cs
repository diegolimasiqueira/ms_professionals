using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the GetProfessionalByIdCommand
/// </summary>
public class GetProfessionalByIdCommandHandler : IRequestHandler<GetProfessionalByIdCommand, GetProfessionalByIdCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalByIdCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalByIdCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the retrieval of a professional by ID
    /// </summary>
    /// <param name="request">Get professional by ID command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional</returns>
    public async Task<GetProfessionalByIdCommandResponse> Handle(GetProfessionalByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException("Professional ID cannot be empty", nameof(request.Id));
            }

            // Get professional
            var professional = await _professionalRepository.GetByIdAsync(request.Id);
            if (professional == null)
            {
                throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
            }

            // Return response
            return new GetProfessionalByIdCommandResponse(professional);
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