namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Represents a profession
/// </summary>
public class Profession
{
    /// <summary>
    /// Profession ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Profession name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Collection of professional professions
    /// </summary>
    public ICollection<ProfessionalProfession> ProfessionalProfessions { get; set; } = new List<ProfessionalProfession>();
} 