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
/// Controller for professionals
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
    /// <param name="mediator">Mediator for command processing</param>
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
    /// <response code="201">Returns the newly created professional</response>
    /// <response code="400">If the professional data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfessionalCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// <returns>The professional if found</returns>
    /// <response code="200">Returns the professional</response>
    /// <response code="404">If the professional is not found</response>
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var command = new GetProfessionalByNameCommand { Name = name };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ProfessionalNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates an existing professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="command">Update professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional</returns>
    /// <response code="200">Returns the updated professional</response>
    /// <response code="400">If the professional data is invalid</response>
    /// <response code="404">If the professional is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateProfessionalCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateProfessionalCommandResponse>> Update(Guid id, UpdateProfessionalCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">If the professional was successfully deleted</response>
    /// <response code="404">If the professional is not found</response>
    /// <response code="400">If the professional cannot be deleted due to constraints</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProfessionalCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates a profession for a professional
    /// </summary>
    /// <param name="command">Create profession command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created profession</returns>
    /// <response code="201">Returns the newly created profession</response>
    /// <response code="400">If the profession data is invalid</response>
    /// <response code="404">If the professional or profession is not found</response>
    [HttpPost("profession")]
    [ProducesResponseType(typeof(CreateProfessionCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProfessionCommandResponse>> CreateProfession(CreateProfessionCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProfessionalId }, result);
    }
} 