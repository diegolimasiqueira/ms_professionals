using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the CreateProfessionalAddressCommand
/// </summary>
public class CreateProfessionalAddressCommandHandler : IRequestHandler<CreateProfessionalAddressCommand, CreateProfessionalAddressCommandResponse>
{
    private readonly IProfessionalAddressRepository _professionalAddressRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICountryCodeRepository _countryCodeRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProfessionalAddressCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    /// <param name="professionalRepository">Professional repository</param>
    /// <param name="countryCodeRepository">Country code repository</param>
    public CreateProfessionalAddressCommandHandler(
        IProfessionalAddressRepository professionalAddressRepository,
        IProfessionalRepository professionalRepository,
        ICountryCodeRepository countryCodeRepository)
    {
        _professionalAddressRepository = professionalAddressRepository;
        _professionalRepository = professionalRepository;
        _countryCodeRepository = countryCodeRepository;
    }

    /// <summary>
    /// Handles the creation of a new professional address
    /// </summary>
    /// <param name="request">Create professional address command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created professional address</returns>
    public async Task<CreateProfessionalAddressCommandResponse> Handle(CreateProfessionalAddressCommand request, CancellationToken cancellationToken)
    {
        // Validate the request using DataAnnotations
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        // Check if professional exists
        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Check if country exists
        var country = await _countryCodeRepository.GetByIdAsync(request.CountryId, cancellationToken);
        if (country == null)
        {
            throw new NotFoundException($"Country with ID {request.CountryId} not found");
        }

        // If this is the default address, unset any existing default address
        if (request.IsDefault)
        {
            var existingDefault = await _professionalAddressRepository.GetDefaultByProfessionalIdAsync(request.ProfessionalId, cancellationToken);
            if (existingDefault != null)
            {
                existingDefault.IsDefault = false;
                await _professionalAddressRepository.UpdateAsync(existingDefault, cancellationToken);
            }
        }

        var professionalAddress = new Domain.Entities.ProfessionalAddress(
            request.ProfessionalId,
            request.StreetAddress,
            request.City,
            request.State,
            request.PostalCode,
            request.Latitude,
            request.Longitude,
            request.IsDefault,
            request.CountryId
        );

        await _professionalAddressRepository.AddAsync(professionalAddress, cancellationToken);

        return new CreateProfessionalAddressCommandResponse(professionalAddress);
    }
} 