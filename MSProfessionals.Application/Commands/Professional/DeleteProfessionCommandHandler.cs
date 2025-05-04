using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the DeleteProfessionCommand
/// </summary>
public class DeleteProfessionCommandHandler : IRequestHandler<DeleteProfessionCommand, Unit>
{
    private readonly IProfessionalProfessionRepository _professionalProfessionRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteProfessionCommandHandler
    /// </summary>
    /// <param name="professionalProfessionRepository">Professional profession repository</param>
    public DeleteProfessionCommandHandler(IProfessionalProfessionRepository professionalProfessionRepository)
    {
        _professionalProfessionRepository = professionalProfessionRepository ?? throw new ArgumentNullException(nameof(professionalProfessionRepository));
    }

    /// <summary>
    /// Handles the deletion of a professional profession
    /// </summary>
    /// <param name="request">Delete profession command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit value</returns>
    public async Task<Unit> Handle(DeleteProfessionCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Professional profession ID cannot be empty", nameof(request.Id));
        }

        // Get professional profession
        var professionalProfession = await _professionalProfessionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (professionalProfession == null)
        {
            throw new ProfessionalProfessionNotFoundException($"Professional profession with ID {request.Id} not found");
        }

        // Delete professional profession
        await _professionalProfessionRepository.DeleteAsync(professionalProfession, cancellationToken);

        return Unit.Value;
    }
} 