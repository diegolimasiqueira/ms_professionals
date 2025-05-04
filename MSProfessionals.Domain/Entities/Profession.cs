using System;
using System.Collections.Generic;

namespace MSProfessionals.Domain.Entities;

/// <summary>
/// Represents a profession
/// </summary>
public class Profession
{
    /// <summary>
    /// Gets or sets the profession ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the profession name
    /// </summary>
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the professional professions
    /// </summary>
    public ICollection<ProfessionalProfession> ProfessionalProfessions { get; set; } = new List<ProfessionalProfession>();
} 