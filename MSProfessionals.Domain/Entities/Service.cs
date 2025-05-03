using System;
using System.Collections.Generic;

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
    /// Service description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Professional services navigation property
    /// </summary>
    public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>();
} 