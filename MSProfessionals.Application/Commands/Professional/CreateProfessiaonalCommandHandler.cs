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
/// Handler for the CreateProfessionalCommand
/// </summary>
public class CreateProfessionalCommandHandler : IRequestHandler<CreateProfessionalCommand, Domain.Entities.Professional>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProfessionalCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public CreateProfessionalCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the creation of a new professional
    /// </summary>
    /// <param name="request">Create professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created professional</returns>
    public async Task<Domain.Entities.Professional> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if email already exists
        var existingProfessional = await _professionalRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingProfessional != null)
        {
            throw new UniqueConstraintViolationException("Email", request.Email);
        }

        var professional = new Domain.Entities.Professional
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DocumentId = request.DocumentId,
            PhotoUrl = request.PhotoUrl,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CurrencyId = request.CurrencyId,
            PhoneCountryCodeId = request.PhoneCountryCodeId,
            PreferredLanguageId = request.PreferredLanguageId,
            TimezoneId = request.TimezoneId,
            SocialMedia = request.SocialMedia,
            Media = request.Media,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _professionalRepository.AddAsync(professional, cancellationToken);
        return professional;
    }
} 