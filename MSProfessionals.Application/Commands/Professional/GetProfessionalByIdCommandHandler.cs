using System;
using System.ComponentModel.DataAnnotations;
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
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the retrieval of a professional by ID
    /// </summary>
    /// <param name="request">Get professional by ID command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional</returns>
    public async Task<GetProfessionalByIdCommandResponse> Handle(GetProfessionalByIdCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        var professional = await _professionalRepository.GetByIdAsync(request.Id);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
        }

        return new GetProfessionalByIdCommandResponse(professional);
    }
} 