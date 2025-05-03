using System;

namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Response for the CreateProfessionCommand
/// </summary>
public class CreateProfessionCommandResponse
{
    /// <summary>
    /// Gets the professional ID
    /// </summary>
    public Guid ProfessionalId { get; }

    /// <summary>
    /// Gets the profession ID
    /// </summary>
    public Guid ProfessionId { get; }

    /// <summary>
    /// Gets a value indicating whether this is the main profession
    /// </summary>
    public bool IsMain { get; }

    /// <summary>
    /// Initializes a new instance of the CreateProfessionCommandResponse
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="professionId">Profession ID</param>
    /// <param name="isMain">Whether this is the main profession</param>
    public CreateProfessionCommandResponse(Guid professionalId, Guid professionId, bool isMain)
    {
        ProfessionalId = professionalId;
        ProfessionId = professionId;
        IsMain = isMain;
    }
} 