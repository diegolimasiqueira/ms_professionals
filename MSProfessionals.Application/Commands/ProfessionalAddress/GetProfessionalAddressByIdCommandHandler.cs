using MediatR;
using MSProfessionals.Application.Common.Exceptions;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the GetProfessionalAddressByIdCommand
/// </summary>
public class GetProfessionalAddressByIdCommandHandler : IRequestHandler<GetProfessionalAddressByIdCommand, GetProfessionalAddressByIdCommandResponse>
{
    private readonly IAddressRepository _addressRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalAddressByIdCommandHandler
    /// </summary>
    /// <param name="addressRepository">Address repository</param>
    public GetProfessionalAddressByIdCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    /// <summary>
    /// Handles the GetProfessionalAddressByIdCommand
    /// </summary>
    /// <param name="request">GetProfessionalAddressByIdCommand</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>GetProfessionalAddressByIdCommandResponse</returns>
    public async Task<GetProfessionalAddressByIdCommandResponse> Handle(GetProfessionalAddressByIdCommand request, CancellationToken cancellationToken)
    {
        var professionalAddress = await _addressRepository.GetByIdAsync(request.Id);

        if (professionalAddress == null)
        {
            throw new ProfessionalAddressNotFoundException(request.Id);
        }

        return new GetProfessionalAddressByIdCommandResponse(professionalAddress);
    }
} 