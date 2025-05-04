using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.Profession;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing professions
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfessionController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ProfessionController
    /// </summary>
    /// <param name="mediator">Mediator</param>
    public ProfessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of professions
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of professions</returns>
    /// <response code="200">Returns the paginated list of professions</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetProfessionsCommandResponse), 200)]
    public async Task<ActionResult<GetProfessionsCommandResponse>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionsCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Name = name
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 