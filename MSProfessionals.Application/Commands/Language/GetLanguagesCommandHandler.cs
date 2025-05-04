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

namespace MSProfessionals.Application.Commands.Language;

/// <summary>
/// Handler for the GetLanguagesCommand
/// </summary>
public class GetLanguagesCommandHandler : IRequestHandler<GetLanguagesCommand, GetLanguagesCommandResponse>
{
    private readonly ILanguageRepository _languageRepository;

    /// <summary>
    /// Initializes a new instance of the GetLanguagesCommandHandler
    /// </summary>
    /// <param name="languageRepository">Language repository</param>
    public GetLanguagesCommandHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
    }

    /// <summary>
    /// Handles the retrieval of paginated languages
    /// </summary>
    /// <param name="request">Get languages command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of languages</returns>
    public async Task<GetLanguagesCommandResponse> Handle(GetLanguagesCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count
        var totalItems = await _languageRepository.CountAsync(cancellationToken);

        // Get paginated items ordered by description
        var items = await _languageRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken: cancellationToken
        );

        // Order by description and map to response items
        var responseItems = items
            .OrderBy(l => l.Description)
            .Select(l => new LanguageItem(
                l.Id,
                l.Code,
                l.Description
            ));

        return new GetLanguagesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            responseItems
        );
    }
} 