using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.ProfessionalService;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for professional services
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfessionalServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessionalServiceController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator</param>
    public ProfessionalServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a professional service by ID
    /// </summary>
    /// <param name="id">Professional service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The professional service if found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetProfessionalServiceByIdCommandResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalServiceByIdCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Adds a new professional service
    /// </summary>
    /// <param name="command">Add professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The added professional service</returns>
    [HttpPost]
    public async Task<ActionResult<AddProfessionalServiceCommandResponse>> Add(AddProfessionalServiceCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing professional service
    /// </summary>
    /// <param name="id">Professional service ID</param>
    /// <param name="command">Update professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional service</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateProfessionalServiceCommandResponse>> Update(Guid id, UpdateProfessionalServiceCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
} 