using MediatR;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Domain.Exceptions;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the GetProfessionalAddressesByProfessionalIdCommand
/// </summary>
public class GetProfessionalAddressesByProfessionalIdCommandHandler : IRequestHandler<GetProfessionalAddressesByProfessionalIdCommand, IEnumerable<GetProfessionalAddressByIdCommandResponse>>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IProfessionalRepository _professionalRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalAddressesByProfessionalIdCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    /// <param name="professionalRepository">Professional repository</param>
    public GetProfessionalAddressesByProfessionalIdCommandHandler(
        IAddressRepository addressRepository,
        IProfessionalRepository professionalRepository)
    {
        _addressRepository = addressRepository;
        _professionalRepository = professionalRepository;
    }

    /// <summary>
    /// Handles the retrieval of professional addresses by professional ID
    /// </summary>
    /// <param name="request">Get professional addresses command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional addresses</returns>
    /// <exception cref="ProfessionalNotFoundException">Thrown when the professional is not found</exception>
    public async Task<IEnumerable<GetProfessionalAddressByIdCommandResponse>> Handle(
        GetProfessionalAddressesByProfessionalIdCommand request,
        CancellationToken cancellationToken)
    {
        // Check if the professional exists
        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
        if (professional == null)
        {
            throw new ProfessionalNotFoundException($"Professional with ID {request.ProfessionalId} not found");
        }

        // Get all addresses for the professional
        var addresses = await _addressRepository.GetByProfessionalIdAsync(request.ProfessionalId);

        // Map to response
        return addresses.Select(address => new GetProfessionalAddressByIdCommandResponse(address));
    }
} 