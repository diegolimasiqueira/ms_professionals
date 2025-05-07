using MediatR;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.Profession;

/// <summary>
/// Handler for the GetProfessionsCommand
/// </summary>
public class GetProfessionsCommandHandler : IRequestHandler<GetProfessionsCommand, GetProfessionsCommandResponse>
{
    private readonly IProfessionRepository _professionRepository;

    /// <summary>
    /// Initializes a new instance of the GetProfessionsCommandHandler
    /// </summary>
    /// <param name="professionRepository">Profession repository</param>
    public GetProfessionsCommandHandler(IProfessionRepository professionRepository)
    {
        _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
    }

    /// <inheritdoc />
    public async Task<GetProfessionsCommandResponse> Handle(GetProfessionsCommand request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        var professions = await _professionRepository.GetAllAsync(skip, request.PageSize, request.Name, cancellationToken);
        var totalCount = await _professionRepository.CountAsync(request.Name, cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetProfessionsCommandResponse
        {
            Items = professions.Select(p => new GetProfessionsCommandResponse.ProfessionItem
            {
                Id = p.Id,
                Name = p.Name
            }),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
} 