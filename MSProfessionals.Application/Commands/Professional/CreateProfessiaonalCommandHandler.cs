using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the CreateProfessionalCommand
/// </summary>
public class CreateProfessionalCommandHandler : IRequestHandler<CreateProfessionalCommand, CreateProfessionalCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProfessionalCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public CreateProfessionalCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the creation of a new professional
    /// </summary>
    /// <param name="request">Create professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created professional</returns>
    public async Task<CreateProfessionalCommandResponse> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Check if professional with same email already exists
        var existingProfessional = await _professionalRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingProfessional != null)
        {
            throw new UniqueConstraintViolationException("Email", request.Email);
        }

        // Create new professional
        var professional = new Domain.Entities.Professional
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DocumentId = request.DocumentId,
            PhotoUrl = request.PhotoUrl,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            SocialMedia = request.SocialMedia,
            Media = request.Media,
            CurrencyId = request.CurrencyId,
            PhoneCountryCodeId = request.PhoneCountryCodeId,
            PreferredLanguageId = request.PreferredLanguageId,
            TimezoneId = request.TimezoneId,
            CreatedAt = DateTime.UtcNow
        };

        // Save professional
        await _professionalRepository.AddAsync(professional, cancellationToken);

        // Return response
        return new CreateProfessionalCommandResponse
        {
            Id = professional.Id,
            Name = professional.Name,
            DocumentId = professional.DocumentId,
            PhotoUrl = professional.PhotoUrl,
            PhoneNumber = professional.PhoneNumber,
            Email = professional.Email,
            SocialMedia = professional.SocialMedia,
            Media = professional.Media,
            CurrencyId = professional.CurrencyId,
            PhoneCountryCodeId = professional.PhoneCountryCodeId,
            PreferredLanguageId = professional.PreferredLanguageId,
            TimezoneId = professional.TimezoneId,
            CreatedAt = professional.CreatedAt,
            UpdatedAt = professional.UpdatedAt
        };
    }
} 