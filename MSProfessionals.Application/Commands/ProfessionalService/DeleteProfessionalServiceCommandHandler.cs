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

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Handler for the DeleteProfessionalServiceCommand
/// </summary>
public class DeleteProfessionalServiceCommandHandler : IRequestHandler<DeleteProfessionalServiceCommand, Unit>
{
    private readonly IProfessionalServiceRepository _professionalServiceRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteProfessionalServiceCommandHandler
    /// </summary>
    /// <param name="professionalServiceRepository">Professional service repository</param>
    public DeleteProfessionalServiceCommandHandler(IProfessionalServiceRepository professionalServiceRepository)
    {
        _professionalServiceRepository = professionalServiceRepository;
    }

    /// <summary>
    /// Handles the deletion of a professional service
    /// </summary>
    /// <param name="request">Delete professional service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit</returns>
    /// <exception cref="ArgumentException">Thrown when the ID is empty</exception>
    /// <exception cref="ProfessionalServiceNotFoundException">Thrown when the professional service is not found</exception>
    public async Task<Unit> Handle(DeleteProfessionalServiceCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Professional service ID cannot be empty", nameof(request.Id));
        }

        var professionalService = await _professionalServiceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (professionalService == null)
        {
            throw new ProfessionalServiceNotFoundException($"Professional service with ID {request.Id} not found");
        }

        try
        {
            await _professionalServiceRepository.DeleteAsync(professionalService, cancellationToken);
            return Unit.Value;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            switch (pgEx.SqlState)
            {
                case "23503": // Foreign key violation
                    throw new InvalidOperationException($"Cannot delete professional service with ID {request.Id} because it has associated records");
                case "23505": // Unique constraint violation
                    throw new InvalidOperationException($"Cannot delete professional service with ID {request.Id} because it violates a unique constraint");
                default:
                    throw new InvalidOperationException($"Error deleting professional service: {pgEx.Message}");
            }
        }
    }
} 