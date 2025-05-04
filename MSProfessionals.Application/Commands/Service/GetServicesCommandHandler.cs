using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Application.Commands.Service;

/// <summary>
/// Handler for the GetServicesCommand
/// </summary>
public class GetServicesCommandHandler : IRequestHandler<GetServicesCommand, GetServicesCommandResponse>
{
    private readonly IServiceRepository _serviceRepository;

    /// <summary>
    /// Initializes a new instance of the GetServicesCommandHandler
    /// </summary>
    /// <param name="serviceRepository">Service repository</param>
    public GetServicesCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
    }

    /// <inheritdoc />
    public async Task<GetServicesCommandResponse> Handle(GetServicesCommand request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        var services = await _serviceRepository.GetAllAsync(skip, request.PageSize, request.Name, cancellationToken);
        var totalCount = await _serviceRepository.CountAsync(request.Name, cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetServicesCommandResponse
        {
            Items = services.Select(s => new GetServicesCommandResponse.ServiceItem
            {
                Id = s.Id,
                Name = s.Name
            }),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
} 