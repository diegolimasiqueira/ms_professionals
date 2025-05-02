namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Represents a service
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
    /// Collection of professional services
    /// </summary>
    public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>();
} 