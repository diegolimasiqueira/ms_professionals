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
/// Handler for the update professional command
/// </summary>
public class UpdateProfessionalCommandHandler : IRequestHandler<UpdateProfessionalCommand, UpdateProfessionalCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateProfessionalCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public UpdateProfessionalCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the update professional command
    /// </summary>
    /// <param name="request">The command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional response</returns>
    /// <exception cref="ArgumentException">Thrown when the ID is empty</exception>
    /// <exception cref="ProfessionalNotFoundException">Thrown when the professional is not found</exception>
    public async Task<UpdateProfessionalCommandResponse> Handle(UpdateProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Professional ID cannot be empty", nameof(request.Id));
        }

        var professional = await _professionalRepository.GetByIdAsync(request.Id);
        
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
        }

        professional.Name = request.Name;
        professional.DocumentId = request.DocumentId;
        professional.PhoneNumber = request.PhoneNumber;
        professional.Email = request.Email;
        professional.CurrencyId = request.CurrencyId;
        professional.PhoneCountryCodeId = request.PhoneCountryCodeId;
        professional.PreferredLanguageId = request.PreferredLanguageId;
        professional.TimezoneId = request.TimezoneId;
        professional.SocialMedia = request.SocialMedia;
        professional.Media = request.Media;
        professional.UpdatedAt = DateTime.UtcNow;

        await _professionalRepository.UpdateAsync(professional);

        return new UpdateProfessionalCommandResponse(professional);
    }
} 