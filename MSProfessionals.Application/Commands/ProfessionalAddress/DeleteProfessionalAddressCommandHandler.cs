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

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the DeleteProfessionalAddressCommand
/// </summary>
public class DeleteProfessionalAddressCommandHandler : IRequestHandler<DeleteProfessionalAddressCommand, Unit>
{
    private readonly IProfessionalAddressRepository _professionalAddressRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteProfessionalAddressCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    public DeleteProfessionalAddressCommandHandler(IProfessionalAddressRepository professionalAddressRepository)
    {
        _professionalAddressRepository = professionalAddressRepository;
    }

    /// <summary>
    /// Handles the deletion of a professional address
    /// </summary>
    /// <param name="request">Delete professional address command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit</returns>
    /// <exception cref="ArgumentException">Thrown when the ID is empty</exception>
    /// <exception cref="ProfessionalNotFoundException">Thrown when the professional address is not found</exception>
    public async Task<Unit> Handle(DeleteProfessionalAddressCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Professional address ID cannot be empty", nameof(request.Id));
        }

        var professionalAddress = await _professionalAddressRepository.GetByIdAsync(request.Id, cancellationToken);
        if (professionalAddress == null)
        {
            throw new ProfessionalNotFoundException($"Professional address with ID {request.Id} not found");
        }

        try
        {
            await _professionalAddressRepository.DeleteAsync(professionalAddress, cancellationToken);
            return Unit.Value;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            switch (pgEx.SqlState)
            {
                case "23503": // Foreign key violation
                    throw new InvalidOperationException($"Cannot delete professional address with ID {request.Id} because it has associated records");
                case "23505": // Unique constraint violation
                    throw new InvalidOperationException($"Cannot delete professional address with ID {request.Id} because it violates a unique constraint");
                default:
                    throw new InvalidOperationException($"Error deleting professional address: {pgEx.Message}");
            }
        }
    }
} 