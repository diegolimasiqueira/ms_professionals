using System;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Response for the GetProfessionalServiceByIdCommand
/// </summary>
public class GetProfessionalServiceByIdCommandResponse
{
    /// <summary>
    /// Professional service ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Professional profession ID
    /// </summary>
    public Guid ProfessionalProfessionId { get; set; }

    /// <summary>
    /// Service ID
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Profession name
    /// </summary>
    public string ProfessionName { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the GetProfessionalServiceByIdCommandResponse
    /// </summary>
    /// <param name="professionalService">Professional service entity</param>
    public GetProfessionalServiceByIdCommandResponse(Domain.Entities.ProfessionalService professionalService)
    {
        Id = professionalService.Id;
        ProfessionalProfessionId = professionalService.ProfessionalProfessionId;
        ServiceId = professionalService.ServiceId;
        ServiceName = professionalService.Service.Name;
        ProfessionName = professionalService.ProfessionalProfession.Profession.Name;
        CreatedAt = professionalService.CreatedAt;
        UpdatedAt = professionalService.UpdatedAt;
    }
} 