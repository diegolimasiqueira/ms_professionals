using MediatR;
using MSProfessionals.Application.Common.Exceptions;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Handler for the GetProfessionalAddressByIdCommand
/// </summary>
public class GetProfessionalAddressByIdCommandHandler : IRequestHandler<GetProfessionalAddressByIdCommand, GetProfessionalAddressByIdCommandResponse>
{
    private readonly IProfessionalAddressRepository _professionalAddressRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionalAddressByIdCommandHandler
    /// </summary>
    /// <param name="professionalAddressRepository">Professional address repository</param>
    public GetProfessionalAddressByIdCommandHandler(IProfessionalAddressRepository professionalAddressRepository)
    {
        _professionalAddressRepository = professionalAddressRepository;
    }

    /// <summary>
    /// Handles the GetProfessionalAddressByIdCommand
    /// </summary>
    /// <param name="request">GetProfessionalAddressByIdCommand</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>GetProfessionalAddressByIdCommandResponse</returns>
    public async Task<GetProfessionalAddressByIdCommandResponse> Handle(GetProfessionalAddressByIdCommand request, CancellationToken cancellationToken)
    {
        var professionalAddress = await _professionalAddressRepository.GetByIdAsync(request.Id, cancellationToken);

        if (professionalAddress == null)
        {
            throw new ProfessionalAddressNotFoundException(request.Id);
        }

        return new GetProfessionalAddressByIdCommandResponse(professionalAddress);
    }
} 