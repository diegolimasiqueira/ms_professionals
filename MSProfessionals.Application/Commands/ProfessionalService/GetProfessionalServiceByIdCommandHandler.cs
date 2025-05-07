using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the GetProfessionalServiceByIdCommand
/// </summary>
public class GetProfessionalServiceByIdCommandHandler : IRequestHandler<GetProfessionalServiceByIdCommand, GetProfessionalServiceByIdCommandResponse>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalServiceByIdCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    public GetProfessionalServiceByIdCommandHandler(IProfessionalServiceRepository professionalServiceRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
    }

    /// <summary>
    /// Handles the retrieval of a professional service by ID
    /// </summary>
    /// <param name="request">Get professional service by ID command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional service</returns>
    public async Task<GetProfessionalServiceByIdCommandResponse> Handle(GetProfessionalServiceByIdCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        var professionalService = await _professionalServiceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (professionalService == null)
        {
            throw new ProfessionalServiceNotFoundException($"Professional service with ID {request.Id} not found");
        }

        return new GetProfessionalServiceByIdCommandResponse(professionalService);
    }
} 