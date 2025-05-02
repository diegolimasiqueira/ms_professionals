using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing professionals
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfessionalsController> _logger;

    /// <summary>
    /// Initializes a new instance of the ProfessionalsController
    /// </summary>
    /// <param name="mediator">Mediator</param>
    /// <param name="logger">Logger</param>
    public ProfessionalsController(IMediator mediator, ILogger<ProfessionalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new professional
    /// </summary>
    /// <param name="command">Create professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created professional</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfessionalCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateProfessionalCommandResponse>> Create(CreateProfessionalCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Gets a professional by ID
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetProfessionalByIdCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProfessionalByIdCommandResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalByIdCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a professional by name
    /// </summary>
    /// <param name="name">Professional name</param>
    /// <returns>The found professional</returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(GetProfessionalByNameCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProfessionalByNameCommandResponse>> GetByName(string name)
    {
        var command = new GetProfessionalByNameCommand { Name = name };
        var professional = await _mediator.Send(command);
        var response = new GetProfessionalByNameCommandResponse(professional);
        return Ok(response);
    }

    /// <summary>
    /// Updates a professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="command">Update professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateProfessionalCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UpdateProfessionalCommandResponse>> Update(Guid id, UpdateProfessionalCommand command, CancellationToken cancellationToken = default)
    {
        if (id != command.Id)
        {
            return BadRequest("The ID in the URL must match the ID in the request body");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProfessionalCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
} 