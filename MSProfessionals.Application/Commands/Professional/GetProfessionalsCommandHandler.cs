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

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the GetProfessionalsCommand
/// </summary>
public class GetProfessionalsCommandHandler : IRequestHandler<GetProfessionalsCommand, GetProfessionalsCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalsCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalsCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the retrieval of paginated professionals
    /// </summary>
    /// <param name="request">Get professionals command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of professionals</returns>
    public async Task<GetProfessionalsCommandResponse> Handle(GetProfessionalsCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count
        var totalItems = await _professionalRepository.CountAsync(cancellationToken);

        // Get paginated items ordered by name
        var items = await _professionalRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken: cancellationToken
        );

        // Order by name and map to response items
        var responseItems = items
            .OrderBy(p => p.Name)
            .Select(p => new ProfessionalItem(
                p.Id,
                p.Name,
                p.DocumentId,
                p.PhotoUrl,
                p.PhoneNumber,
                p.Email,
                p.SocialMedia,
                p.Media,
                p.CurrencyId,
                p.PhoneCountryCodeId,
                p.PreferredLanguageId,
                p.TimezoneId,
                p.CreatedAt,
                p.UpdatedAt
            ));

        return new GetProfessionalsCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            responseItems
        );
    }
} 