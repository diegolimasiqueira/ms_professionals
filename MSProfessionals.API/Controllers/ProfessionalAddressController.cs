using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using System.ComponentModel.DataAnnotations;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing professional addresses
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfessionalAddressController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ProfessionalAddressController
    /// </summary>
    /// <param name="mediator">Mediator for command processing</param>
    public ProfessionalAddressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new professional address
    /// </summary>
    /// <param name="command">Professional address data</param>
    /// <returns>The created professional address</returns>
    /// <response code="201">Returns the newly created professional address</response>
    /// <response code="400">If the professional address data is invalid</response>
    /// <response code="404">If the professional or country is not found</response>
    /// <response code="500">If an internal error occurs</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfessionalAddressCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProfessionalAddressCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Gets a professional address by ID
    /// </summary>
    /// <param name="id">Professional address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The found professional address</returns>
    /// <response code="200">Returns the found professional address</response>
    /// <response code="404">If the professional address is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetProfessionalAddressByIdCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalAddressByIdCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all addresses for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional addresses</returns>
    /// <response code="200">Returns the list of addresses</response>
    /// <response code="404">If the professional is not found</response>
    [HttpGet("professional/{professionalId}")]
    [ProducesResponseType(typeof(IEnumerable<GetProfessionalAddressByIdCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProfessionalId([Required] Guid professionalId, CancellationToken cancellationToken = default)
    {
        var command = new GetProfessionalAddressesByProfessionalIdCommand { ProfessionalId = professionalId };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates a professional address
    /// </summary>
    /// <param name="command">Professional address data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional address</returns>
    /// <response code="200">Returns the updated professional address</response>
    /// <response code="400">If the professional address data is invalid</response>
    /// <response code="404">If the professional address or country is not found</response>
    [HttpPut]
    [ProducesResponseType(typeof(UpdateProfessionalAddressCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateProfessionalAddressCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a professional address
    /// </summary>
    /// <param name="id">Professional address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">If the professional address was successfully deleted</response>
    /// <response code="404">If the professional address is not found</response>
    /// <response code="400">If the professional address cannot be deleted due to constraints</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProfessionalAddressCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
} 