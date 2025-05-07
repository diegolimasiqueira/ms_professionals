namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Country entity
/// </summary>
public class Country
{
    /// <summary>
    /// Country ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country code
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Professional addresses navigation property
    /// </summary>
    public ICollection<ProfessionalAddress> ProfessionalAddresses { get; set; } = new List<ProfessionalAddress>();
} 