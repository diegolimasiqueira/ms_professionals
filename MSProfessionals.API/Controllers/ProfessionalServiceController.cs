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
    /// Initializes a new instance of the ProfessionalServiceController
    /// </summary>
    /// <param name="mediator">Mediator for command processing</param>
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
    /// <response code="200">Returns the professional service</response>
    /// <response code="404">If the professional service is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetProfessionalServiceByIdCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProfessionalServiceByIdCommandResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalServiceByIdCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all services for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional services</returns>
    /// <response code="200">Returns the list of services</response>
    /// <response code="404">If the professional is not found</response>
    [HttpGet("professional/{professionalId}")]
    [ProducesResponseType(typeof(IEnumerable<GetProfessionalServiceByIdCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<GetProfessionalServiceByIdCommandResponse>>> GetByProfessionalId(Guid professionalId, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalServicesByProfessionalIdCommand { ProfessionalId = professionalId };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Adds a new professional service
    /// </summary>
    /// <param name="command">Add professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The added professional service</returns>
    /// <response code="201">Returns the newly created professional service</response>
    /// <response code="400">If the professional service data is invalid</response>
    /// <response code="404">If the professional, profession, or service is not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(AddProfessionalServiceCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// <response code="200">Returns the updated professional service</response>
    /// <response code="400">If the professional service data is invalid</response>
    /// <response code="404">If the professional service, profession, or service is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateProfessionalServiceCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateProfessionalServiceCommandResponse>> Update(Guid id, UpdateProfessionalServiceCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a professional service
    /// </summary>
    /// <param name="id">Professional service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">If the professional service was successfully deleted</response>
    /// <response code="404">If the professional service is not found</response>
    /// <response code="400">If the professional service cannot be deleted due to constraints</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProfessionalServiceCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
} 