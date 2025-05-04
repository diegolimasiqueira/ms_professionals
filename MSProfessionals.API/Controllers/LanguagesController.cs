using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.Language;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing languages
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LanguagesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the LanguagesController
    /// </summary>
    /// <param name="mediator">Mediator for handling commands</param>
    public LanguagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of languages ordered by description
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of languages</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetLanguagesCommandResponse), 200)]
    public async Task<IActionResult> GetLanguages(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetLanguagesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of languages filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of filtered languages</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(GetLanguagesCommandResponse), 200)]
    public async Task<IActionResult> GetLanguagesByDescription(
        [FromQuery] string description,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var command = new GetLanguagesByDescriptionCommand
        {
            Description = description,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 