namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Service entity
/// </summary>
public class Service
{
    /// <summary>
    /// Service ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Service name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Professional services navigation property
    /// </summary>
    public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>();
} 