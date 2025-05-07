using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.TimeZone;

/// <summary>
/// Handler for the GetTimeZonesCommand
/// </summary>
public class GetTimeZonesCommandHandler : IRequestHandler<GetTimeZonesCommand, GetTimeZonesCommandResponse>
{
    private readonly ITimeZoneRepository _timeZoneRepository;

    /// <summary>
    /// Initializes a new instance of the GetTimeZonesCommandHandler
    /// </summary>
    /// <param name="timeZoneRepository">Time zone repository</param>
    public GetTimeZonesCommandHandler(ITimeZoneRepository timeZoneRepository)
    {
        _timeZoneRepository = timeZoneRepository ?? throw new ArgumentNullException(nameof(timeZoneRepository));
    }

    /// <summary>
    /// Handles the retrieval of paginated time zones
    /// </summary>
    /// <param name="request">Get time zones command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of time zones</returns>
    public async Task<GetTimeZonesCommandResponse> Handle(GetTimeZonesCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Get total count
        var totalItems = await _timeZoneRepository.CountAsync(cancellationToken);

        // Get paginated items ordered by description
        var items = await _timeZoneRepository.GetAllAsync(
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