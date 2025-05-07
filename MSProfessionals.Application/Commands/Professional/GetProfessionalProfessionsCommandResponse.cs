namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Response for the GetProfessionalProfessionsCommand
/// </summary>
public class GetProfessionalProfessionsCommandResponse
{
    /// <summary>
    /// Gets the profession ID
    /// </summary>
    public Guid ProfessionId { get; }

    /// <summary>
    /// Gets the professional ID
    /// </summary>
    public Guid ProfessionalId { get; }

    /// <summary>
    /// Gets the profession name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the GetProfessionalProfessionsCommandResponse
    /// </summary>
    /// <param name="professionId">Profession ID</param>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="name">Profession name</param>
    public GetProfessionalProfessionsCommandResponse(Guid professionId, Guid professionalId, string name)
    {
        ProfessionId = professionId;
        ProfessionalId = professionalId;
        Name = name;
    }
} 