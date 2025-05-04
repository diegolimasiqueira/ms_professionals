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

namespace MSProfessionals.Application.Commands.TimeZone;

/// <summary>
/// Handler for the GetTimeZonesByDescriptionCommand
/// </summary>
public class GetTimeZonesByDescriptionCommandHandler : IRequestHandler<GetTimeZonesByDescriptionCommand, GetTimeZonesCommandResponse>
{
    private readonly ITimeZoneRepository _timeZoneRepository;

    /// <summary>
    /// Initializes a new instance of the GetTimeZonesByDescriptionCommandHandler
    /// </summary>
    /// <param name="timeZoneRepository">Time zone repository</param>
    public GetTimeZonesByDescriptionCommandHandler(ITimeZoneRepository timeZoneRepository)
    {
        _timeZoneRepository = timeZoneRepository ?? throw new ArgumentNullException(nameof(timeZoneRepository));
    }

    /// <summary>
    /// Handles the retrieval of time zones filtered by description
    /// </summary>
    /// <param name="request">Get time zones by description command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of filtered time zones</returns>
    public async Task<GetTimeZonesCommandResponse> Handle(GetTimeZonesByDescriptionCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count of filtered items
        var totalItems = await _timeZoneRepository.CountByDescriptionAsync(request.Description, cancellationToken);

        // Get paginated items filtered by description and ordered by description
        var items = await _timeZoneRepository.GetByDescriptionAsync(
            request.Description,
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken: cancellationToken
        );

        // Order by description and map to response items
        var responseItems = items
            .OrderBy(tz => tz.Description)
            .Select(tz => new TimeZoneItem(
                tz.Id,
                tz.Name,
                tz.Description
            ));

        return new GetTimeZonesCommandResponse(
            request.PageNumber,
            request.PageSize,
            totalItems,
            responseItems
        );
    }
} 