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

namespace MSProfessionals.Application.Commands.Currency;

/// <summary>
/// Handler for the GetCurrenciesCommand
/// </summary>
public class GetCurrenciesCommandHandler : IRequestHandler<GetCurrenciesCommand, GetCurrenciesCommandResponse>
{
    private readonly ICurrencyRepository _currencyRepository;

    /// <summary>
    /// Initializes a new instance of the GetCurrenciesCommandHandler
    /// </summary>
    /// <param name="currencyRepository">Currency repository</param>
    public GetCurrenciesCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    /// <summary>
    /// Handles the retrieval of paginated currencies
    /// </summary>
    /// <param name="request">Get currencies command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of currencies</returns>
    public async Task<GetCurrenciesCommandResponse> Handle(GetCurrenciesCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count
        var totalItems = await _currencyRepository.CountAsync(cancellationToken);

        // Get paginated items ordered by description
        var items = await _currencyRepository.GetAllAsync(
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken: cancellationToken
        );

        // Order by description and map to response items
        var responseItems = items
            .OrderBy(c => c.Description)
            .Select(c => new CurrencyItem(
                c.Id,
                c.Code,
                c.Description
            ));

        return new GetCurrenciesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            responseItems
        );
    }
} 