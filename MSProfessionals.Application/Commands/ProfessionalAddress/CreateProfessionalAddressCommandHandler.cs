using System.ComponentModel.DataAnnotations;
using MediatR;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the CreateProfessionalAddressCommand
/// </summary>
public class CreateProfessionalAddressCommandHandler : IRequestHandler<CreateProfessionalAddressCommand, CreateProfessionalAddressCommandResponse>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICountryCodeRepository _countryCodeRepository;

    /// <summary>
    /// Initializes a new instance of the CreateProfessionalAddressCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    /// <param name="professionalRepository">Professional repository</param>
    /// <param name="countryCodeRepository">Country code repository</param>
    public CreateProfessionalAddressCommandHandler(
        IAddressRepository addressRepository,
        IProfessionalRepository professionalRepository,
        ICountryCodeRepository countryCodeRepository)
    {
        _addressRepository = addressRepository;
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
        var professional = await _professionalRepository.GetByIdWithoutRelationsAsync(request.ProfessionalId, cancellationToken);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Check if country exists
        var country = await _countryCodeRepository.GetByIdAsync(request.CountryId);
        if (country == null)
        {
            throw new NotFoundException($"Country with ID {request.CountryId} not found");
        }

        // If this is the default address, unset any existing default address
        if (request.IsDefault)
        {
            var existingDefault = await _addressRepository.GetDefaultByProfessionalIdAsync(request.ProfessionalId);
            if (existingDefault != null)
            {
                existingDefault.IsDefault = false;
                await _addressRepository.UpdateAsync(existingDefault);
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

        await _addressRepository.AddAsync(professionalAddress);

        return new CreateProfessionalAddressCommandResponse(professionalAddress);
    }
} 