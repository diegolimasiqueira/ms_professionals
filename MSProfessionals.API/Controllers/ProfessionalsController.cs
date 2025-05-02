using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Application.DTOs;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.API.Controllers;

/// <summary>
/// Controller for managing professionals
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ProfessionalsController
    /// </summary>
    /// <param name="mediator">Mediator for handling commands</param>
    public ProfessionalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new professional
    /// </summary>
    /// <param name="command">Professional data</param>
    /// <returns>Created professional</returns>
    /// <response code="201">Returns the newly created professional</response>
    /// <response code="400">If the professional data is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfessionalResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProfessionalCommand command)
    {
        var professional = await _mediator.Send(command);
        var response = MapToDto(professional);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Gets a professional by ID
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <returns>Professional data</returns>
    /// <response code="200">Returns the requested professional</response>
    /// <response code="404">If the professional is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfessionalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var command = new GetProfessionalByIdCommand { Id = id };
        var professional = await _mediator.Send(command);
        if (professional == null)
            return NotFound();

        var response = MapToDto(professional);
        return Ok(response);
    }

    /// <summary>
    /// Gets a professional by name
    /// </summary>
    /// <param name="name">Professional name</param>
    /// <returns>Professional data</returns>
    /// <response code="200">Returns the requested professional</response>
    /// <response code="404">If the professional is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(ProfessionalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetProfessionalByNameCommand { Name = name };
        var professional = await _mediator.Send(command);
        if (professional == null)
            return NotFound();

        var response = MapToDto(professional);
        return Ok(response);
    }

    /// <summary>
    /// Updates a professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="command">Professional data</param>
    /// <returns>Updated professional</returns>
    /// <response code="200">Returns the updated professional</response>
    /// <response code="400">If the professional data is invalid</response>
    /// <response code="404">If the professional is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProfessionalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProfessionalCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var professional = await _mediator.Send(command);
        if (professional == null)
            return NotFound();

        var response = MapToDto(professional);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a professional
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the professional was successfully deleted</response>
    /// <response code="404">If the professional is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProfessionalCommand { Id = id };
        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    private static ProfessionalResponseDto MapToDto(Professional professional)
    {
        return new ProfessionalResponseDto
        {
            Id = professional.Id,
            Name = professional.Name,
            DocumentId = professional.DocumentId,
            PhotoUrl = professional.PhotoUrl,
            PhoneNumber = professional.PhoneNumber,
            Email = professional.Email,
            SocialMedia = professional.SocialMedia,
            Media = professional.Media,
            CurrencyId = professional.CurrencyId,
            PhoneCountryCodeId = professional.PhoneCountryCodeId,
            PreferredLanguageId = professional.PreferredLanguageId,
            TimezoneId = professional.TimezoneId,
            CreatedAt = professional.CreatedAt,
            UpdatedAt = professional.UpdatedAt
        };
    }
} 