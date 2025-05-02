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
/// Handler for the DeleteProfessionalCommand
/// </summary>
public class DeleteProfessionalCommandHandler : IRequestHandler<DeleteProfessionalCommand, bool>
{
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteProfessionalCommandHandler
    /// </summary>
    /// <param name="professionalRepository">Professional repository</param>
    public DeleteProfessionalCommandHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the deletion of a professional
    /// </summary>
    /// <param name="request">Delete professional command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the professional was deleted</returns>
    /// <exception cref="ArgumentException">Thrown when the ID is empty</exception>
    /// <exception cref="ProfessionalNotFoundException">Thrown when the professional is not found</exception>
    public async Task<bool> Handle(DeleteProfessionalCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Professional ID cannot be empty", nameof(request.Id));
        }

        var professional = await _professionalRepository.GetByIdAsync(request.Id);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.Id} not found");
        }

        try
        {
            await _professionalRepository.DeleteAsync(request.Id);
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            switch (pgEx.SqlState)
            {
                case "23503": // Foreign key violation
                    throw new InvalidOperationException($"Cannot delete professional with ID {request.Id} because it has associated records");
                case "23505": // Unique constraint violation
                    throw new InvalidOperationException($"Cannot delete professional with ID {request.Id} because it violates a unique constraint");
                default:
                    throw new InvalidOperationException($"Error deleting professional: {pgEx.Message}");
            }
        }
    }
} 