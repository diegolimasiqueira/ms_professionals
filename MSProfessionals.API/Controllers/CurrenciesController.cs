using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.Currency;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing currencies
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CurrenciesController
    /// </summary>
    /// <param name="mediator">Mediator for handling commands</param>
    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of currencies ordered by description
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of currencies</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetCurrenciesCommandResponse), 200)]
    public async Task<IActionResult> GetCurrencies(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetCurrenciesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of currencies filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of filtered currencies</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(GetCurrenciesCommandResponse), 200)]
    public async Task<IActionResult> GetCurrenciesByDescription(
        [FromQuery] string description,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetCurrenciesByDescriptionCommand
        {
            Description = description,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 