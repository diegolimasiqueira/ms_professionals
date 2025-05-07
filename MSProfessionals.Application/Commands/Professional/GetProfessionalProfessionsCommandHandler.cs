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

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the GetProfessionalProfessionsCommand
/// </summary>
public class GetProfessionalProfessionsCommandHandler : IRequestHandler<GetProfessionalProfessionsCommand, IEnumerable<GetProfessionalProfessionsCommandResponse>>
{
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalProfessionsCommandHandler
    /// </summary>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalProfessionsCommandHandler(
        IProfessionalProfessionRepository professionalProfessionRepository,
        IProfessionalRepository professionalRepository)
    {
        _professionalProfessionRepository = professionalProfessionRepository ?? throw new ArgumentNullException(nameof(professionalProfessionRepository));
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the retrieval of professional professions
    /// </summary>
    /// <param name="request">Get professional professions command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional professions</returns>
    public async Task<IEnumerable<GetProfessionalProfessionsCommandResponse>> Handle(GetProfessionalProfessionsCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        if (request.ProfessionalId == Guid.Empty)
        {
            throw new InvalidOperationException("Professional ID cannot be empty");
        }

        // Check if professional exists
        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId, cancellationToken);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Get professional professions
        var professionalProfessions = await _professionalProfessionRepository.GetByProfessionalIdAsync(request.ProfessionalId, cancellationToken);

        return professionalProfessions.Select(pp => new GetProfessionalProfessionsCommandResponse(
            pp.ProfessionId,
            pp.ProfessionalId,
            pp.Profession.Name
        ));
    }
} 