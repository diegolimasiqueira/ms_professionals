using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.Currency;

/// <summary>
/// Handler for the GetCurrenciesByDescriptionCommand
/// </summary>
public class GetCurrenciesByDescriptionCommandHandler : IRequestHandler<GetCurrenciesByDescriptionCommand, GetCurrenciesCommandResponse>
{
    private readonly ICurrencyRepository _currencyRepository;

    /// <summary>
    /// Initializes a new instance of the GetCurrenciesByDescriptionCommandHandler
    /// </summary>
    /// <param name="currencyRepository">Currency repository</param>
    public GetCurrenciesByDescriptionCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    /// <inheritdoc />
    public async Task<GetCurrenciesCommandResponse> Handle(GetCurrenciesByDescriptionCommand request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        var take = request.PageSize;

        var totalItems = await _currencyRepository.CountByDescriptionAsync(request.Description, cancellationToken);
        var items = await _currencyRepository.GetByDescriptionAsync(request.Description, skip, take, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        return new GetCurrenciesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            items.Select(c => new CurrencyItem(
                c.Id,
                c.Code,
                c.Description
            )).ToList()
        );
    }
} 