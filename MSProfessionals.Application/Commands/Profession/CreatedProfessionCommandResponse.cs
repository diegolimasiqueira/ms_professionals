using MSProfessionals.Domain.Entities;
namespace MSProfessionals.Application.Commands.Profession;

/// <summary>
/// Response for the AddProfessionCommand
/// </summary>
public class CreatedProfessionCommandResponse
{
    /// <summary>
    /// The added profession
    /// </summary>
    public Domain.Entities.Profession Profession { get; }

    /// <summary>
    /// Initializes a new instance of the AddProfessionCommandResponse
    /// </summary>
    /// <param name="profession">The added profession</param>
    public CreatedProfessionCommandResponse(Domain.Entities.Profession profession)
    {
        Profession = profession;
    }
} 