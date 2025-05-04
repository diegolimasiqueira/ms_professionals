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

namespace MSProfessionals.Application.Commands.CountryCode;

/// <summary>
/// Handler for the GetCountryCodesCommand
/// </summary>
public class GetCountryCodesCommandHandler : IRequestHandler<GetCountryCodesCommand, GetCountryCodesCommandResponse>
{
    private readonly ICountryCodeRepository _countryCodeRepository;

    /// <summary>
    /// Initializes a new instance of the GetCountryCodesCommandHandler
    /// </summary>
    /// <param name="countryCodeRepository">Country code repository</param>
    public GetCountryCodesCommandHandler(ICountryCodeRepository countryCodeRepository)
    {
        _countryCodeRepository = countryCodeRepository ?? throw new ArgumentNullException(nameof(countryCodeRepository));
    }

    /// <summary>
    /// Handles the retrieval of paginated country codes
    /// </summary>
    /// <param name="request">Get country codes command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of country codes</returns>
    public async Task<GetCountryCodesCommandResponse> Handle(GetCountryCodesCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count
        var totalItems = await _countryCodeRepository.CountAsync(cancellationToken);

        // Get paginated items ordered by code
        var items = await _countryCodeRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken: cancellationToken
        );

        // Order by code and map to response items
        var responseItems = items
            .OrderBy(cc => cc.Code)
            .Select(cc => new CountryCodeItem(
                cc.Id,
                cc.Code,
                cc.CountryName
            ));

        return new GetCountryCodesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            responseItems
        );
    }
} 