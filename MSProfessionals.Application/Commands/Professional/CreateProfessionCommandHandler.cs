using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the CreateProfessionCommand
/// </summary>
public class CreateProfessionCommandHandler : IRequestHandler<CreateProfessionCommand, CreateProfessionCommandResponse>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IProfessionRepository _professionRepository;
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProfessionCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    /// <param name="professionRepository">Profession repository</param>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    public CreateProfessionCommandHandler(
        IProfessionalRepository professionalRepository,
        IProfessionRepository professionRepository,
        IProfessionalProfessionRepository professionalProfessionRepository)
    {
        _professionalRepository = professionalRepository;
        _professionRepository = professionRepository;
        _professionalProfessionRepository = professionalProfessionRepository;
    }

    /// <summary>
    /// Handles the CreateProfessionCommand
    /// </summary>
    /// <param name="request">Create profession command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created profession</returns>
    public async Task<CreateProfessionCommandResponse> Handle(CreateProfessionCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationContext = new ValidationContext(request);
        Validator.ValidateObject(request, validationContext, true);

        // Check if the professional exists
        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Check if the profession exists
        var profession = await _professionRepository.GetByIdAsync(request.ProfessionId, cancellationToken);
        if (profession == null)
        {
            throw new ProfessionNotFoundException($"Profession with ID {request.ProfessionId} not found");
        }

        try
        {
            // If this is the main profession, unset any existing main profession
            if (request.IsMain)
            {
                var existingMain = await _professionalProfessionRepository.GetMainByProfessionalIdAsync(request.ProfessionalId, cancellationToken);
                if (existingMain != null)
                {
                    existingMain.IsMain = false;
                    await _professionalProfessionRepository.UpdateAsync(existingMain, cancellationToken);
                }
            }

            // Create the professional profession
            var professionalProfession = new ProfessionalProfession
            {
                ProfessionalId = request.ProfessionalId,
                ProfessionId = request.ProfessionId,
                IsMain = request.IsMain
            };

            await _professionalProfessionRepository.AddAsync(professionalProfession, cancellationToken);
            await _professionalProfessionRepository.SaveChangesAsync(cancellationToken);

            return new CreateProfessionCommandResponse(
                professionalProfession.ProfessionalId,
                professionalProfession.ProfessionId,
                professionalProfession.IsMain);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            switch (pgEx.SqlState)
            {
                case "23503": // Foreign key violation
                    throw new InvalidOperationException("The professional or profession does not exist.");
                case "23505": // Unique constraint violation
                    throw new InvalidOperationException("The professional already has this profession.");
                default:
                    throw;
            }
        }
    }
} 