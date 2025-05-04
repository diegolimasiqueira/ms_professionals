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
/// Handler for the UpdateProfessionalAddressCommand
/// </summary>
public class UpdateProfessionalAddressCommandHandler : IRequestHandler<UpdateProfessionalAddressCommand, UpdateProfessionalAddressCommandResponse>
{
    private readonly IAddressRepository _addressRepository;
    private readonly ICountryCodeRepository _countryCodeRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateProfessionalAddressCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    /// <param name="countryCodeRepository">Country code repository</param>
    public UpdateProfessionalAddressCommandHandler(
        IAddressRepository addressRepository,
        ICountryCodeRepository countryCodeRepository)
    {
        _addressRepository = addressRepository;
        _countryCodeRepository = countryCodeRepository;
    }

    /// <summary>
    /// Handles the update of a professional address
    /// </summary>
    /// <param name="request">Update professional address command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated professional address</returns>
    public async Task<UpdateProfessionalAddressCommandResponse> Handle(UpdateProfessionalAddressCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if the professional address exists
        var professionalAddress = await _addressRepository.GetByIdAsync(request.Id);
        if (professionalAddress == null)
        {
            throw new ProfessionalNotFoundException($"Professional address with ID {request.Id} not found");
        }

        // Check if the country exists
        var country = await _countryCodeRepository.GetByIdAsync(request.CountryId);
        if (country == null)
        {
            throw new NotFoundException($"Country with ID {request.CountryId} not found");
        }

        try
        {
            // If this is the default address, unset any existing default address
            if (request.IsDefault)
            {
                var existingDefault = await _addressRepository.GetDefaultByProfessionalIdAsync(professionalAddress.ProfessionalId);
                if (existingDefault != null && existingDefault.Id != request.Id)
                {
                    existingDefault.IsDefault = false;
                    await _addressRepository.UpdateAsync(existingDefault);
                }
            }

            // Update the professional address
            professionalAddress.StreetAddress = request.StreetAddress;
            professionalAddress.City = request.City;
            professionalAddress.State = request.State;
            professionalAddress.PostalCode = request.PostalCode;
            professionalAddress.Latitude = request.Latitude;
            professionalAddress.Longitude = request.Longitude;
            professionalAddress.IsDefault = request.IsDefault;
            professionalAddress.CountryId = request.CountryId;
            professionalAddress.UpdatedAt = DateTime.UtcNow;

            await _addressRepository.UpdateAsync(professionalAddress);

            return new UpdateProfessionalAddressCommandResponse(professionalAddress);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                switch (pgEx.SqlState)
                {
                    case "23503": // Foreign key violation
                        throw new InvalidOperationException($"Cannot update professional address with ID {request.Id} because the referenced entity does not exist");
                    case "23505": // Unique constraint violation
                        throw new InvalidOperationException($"Cannot update professional address with ID {request.Id} because it violates a unique constraint");
                    default:
                        throw;
                }
            }
            throw;
        }
    }
} 