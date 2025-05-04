using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.Service;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing services
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ServiceController
    /// </summary>
    /// <param name="mediator">Mediator</param>
    public ServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of services
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of services</returns>
    /// <response code="200">Returns the paginated list of services</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetServicesCommandResponse), 200)]
    public async Task<ActionResult<GetServicesCommandResponse>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetServicesCommand
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Name = name
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 