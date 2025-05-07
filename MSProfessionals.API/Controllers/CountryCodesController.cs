using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.CountryCode;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing country codes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CountryCodesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CountryCodesController
    /// </summary>
    /// <param name="mediator">Mediator for handling commands</param>
    public CountryCodesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of country codes
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of country codes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetCountryCodesCommandResponse), 200)]
    public async Task<ActionResult<GetCountryCodesCommandResponse>> GetCountryCodes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetCountryCodesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of country codes filtered by country name
    /// </summary>
    /// <param name="countryName">Country name to filter by</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of filtered country codes</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(GetCountryCodesCommandResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GetCountryCodesCommandResponse>> GetCountryCodesByCountryName(
        [FromQuery] string countryName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetCountryCodesByCountryNameCommand
        {
            CountryName = countryName,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 