using System;

namespace MSProfessionals.Application.Commands.ProfessionalService;

/// <summary>
/// Response for the AddProfessionalServiceCommand
/// </summary>
public class AddProfessionalServiceCommandResponse
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
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the AddProfessionalServiceCommandResponse
    /// </summary>
    /// <param name="professionalService">Professional service entity</param>
    public AddProfessionalServiceCommandResponse(Domain.Entities.ProfessionalService professionalService)
    {
        Id = professionalService.Id;
        ProfessionalProfessionId = professionalService.ProfessionalProfessionId;
        ServiceId = professionalService.ServiceId;
        CreatedAt = professionalService.CreatedAt;
        UpdatedAt = professionalService.UpdatedAt;
    }
} 