using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.TimeZone;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing time zones
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TimeZonesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the TimeZonesController
    /// </summary>
    /// <param name="mediator">Mediator for handling commands</param>
    public TimeZonesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of time zones ordered by description
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of time zones</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetTimeZonesCommandResponse), 200)]
    public async Task<IActionResult> GetTimeZones(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetTimeZonesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of time zones filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of filtered time zones</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(GetTimeZonesCommandResponse), 200)]
    public async Task<IActionResult> GetTimeZonesByDescription(
        [FromQuery] string description,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetTimeZonesByDescriptionCommand
        {
            Description = description,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 