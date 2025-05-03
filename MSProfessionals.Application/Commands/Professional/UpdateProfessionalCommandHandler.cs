using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the UpdateProfessionalCommand
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
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the update of a professional
    /// </summary>
    /// <param name="request">Update professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional</returns>
    public async Task<UpdateProfessionalCommandResponse> Handle(UpdateProfessionalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Get existing professional
            var professional = await _professionalRepository.GetByIdAsync(request.Id);
            if (professional == null)
            {
                throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
            }

            // Check if email is being changed and if it already exists
            if (professional.Email != request.Email)
            {
                var existingProfessional = await _professionalRepository.GetByEmailAsync(request.Email, cancellationToken);
                if (existingProfessional != null)
                {
                    throw new UniqueConstraintViolationException("Email", request.Email);
                }
            }

            // Update professional
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

            // Save changes
            await _professionalRepository.UpdateAsync(professional);

            // Return response
            return new UpdateProfessionalCommandResponse(professional);
        }
        catch (System.ComponentModel.DataAnnotations.ValidationException ex)
        {
            throw new Domain.Exceptions.ValidationException($"Validation failed: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Failed to update professional due to database error", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while updating the professional", ex);
        }
    }
} 