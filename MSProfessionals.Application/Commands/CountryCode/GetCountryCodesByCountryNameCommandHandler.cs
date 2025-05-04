using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.CountryCode;

/// <summary>
/// Handler for the GetCountryCodesByCountryNameCommand
/// </summary>
public class GetCountryCodesByCountryNameCommandHandler : IRequestHandler<GetCountryCodesByCountryNameCommand, GetCountryCodesCommandResponse>
{
    private readonly ICountryCodeRepository _countryCodeRepository;

    /// <summary>
    /// Initializes a new instance of the GetCountryCodesByCountryNameCommandHandler
    /// </summary>
    /// <param name="countryCodeRepository">Country code repository</param>
    public GetCountryCodesByCountryNameCommandHandler(ICountryCodeRepository countryCodeRepository)
    {
        _countryCodeRepository = countryCodeRepository;
    }

    /// <inheritdoc />
    public async Task<GetCountryCodesCommandResponse> Handle(GetCountryCodesByCountryNameCommand request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        var take = request.PageSize;

        var totalItems = await _countryCodeRepository.CountByCountryNameAsync(request.CountryName, cancellationToken);
        var items = await _countryCodeRepository.GetByCountryNameAsync(request.CountryName, skip, take, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        return new GetCountryCodesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            items.Select(cc => new CountryCodeItem(
                cc.Id,
                cc.Code,
                cc.CountryName
            )).ToList()
        );
    }
} 