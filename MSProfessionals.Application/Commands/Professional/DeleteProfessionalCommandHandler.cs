using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Handler for the DeleteProfessionalCommand
/// </summary>
public class DeleteProfessionalCommandHandler : IRequestHandler<DeleteProfessionalCommand, Unit>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteProfessionalCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public DeleteProfessionalCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository ?? throw new ArgumentNullException(nameof(professionalRepository));
    }

    /// <summary>
    /// Handles the deletion of a professional
    /// </summary>
    /// <param name="request">Delete professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit value</returns>
    public async Task<Unit> Handle(DeleteProfessionalCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            throw new InvalidOperationException("Professional ID cannot be empty");
        }

        // Get professional
        var professional = await _professionalRepository.GetByIdAsync(request.Id);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
        }

        // Delete professional
        await _professionalRepository.DeleteAsync(request.Id);

        return Unit.Value;
    }
} 