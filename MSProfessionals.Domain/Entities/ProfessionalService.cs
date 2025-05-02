namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Represents the relationship between a professional profession and a service
/// </summary>
public class ProfessionalService
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
    /// Professional profession
    /// </summary>
    public ProfessionalProfession ProfessionalProfession { get; set; } = null!;

    /// <summary>
    /// Service ID
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Service
    /// </summary>
    public Service Service { get; set; } = null!;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
} 