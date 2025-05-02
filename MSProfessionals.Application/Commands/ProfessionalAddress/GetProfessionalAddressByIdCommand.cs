using System;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command to get a professional address by ID
/// </summary>
public class GetProfessionalAddressByIdCommand : IRequest<GetProfessionalAddressByIdCommandResponse>
{
    /// <summary>
    /// Professional address ID
    /// </summary>
    public Guid Id { get; set; }
} 