namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Represents the relationship between a professional and a profession
/// </summary>
public class ProfessionalProfession
{
    /// <summary>
    /// Professional profession ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Professional ID
    /// </summary>
    public Guid ProfessionalId { get; set; }

    /// <summary>
    /// Professional
    /// </summary>
    public Professional Professional { get; set; } = null!;

    /// <summary>
    /// Profession ID
    /// </summary>
    public Guid ProfessionId { get; set; }

    /// <summary>
    /// Profession
    /// </summary>
    public Profession Profession { get; set; } = null!;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Collection of professional services
    /// </summary>
    public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>();

    /// <summary>
    /// Initializes a new instance of the ProfessionalProfession
    /// </summary>
    public ProfessionalProfession(Guid professionalId, Guid professionId)
    {
        Id = Guid.NewGuid();
        ProfessionalId = professionalId;
        ProfessionId = professionId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
} 