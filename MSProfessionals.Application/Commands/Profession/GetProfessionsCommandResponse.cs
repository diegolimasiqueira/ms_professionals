using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.Profession;

/// <summary>
/// Response for the GetProfessionsCommand
/// </summary>
public class GetProfessionsCommandResponse
{
    /// <summary>
    /// List of professions
    /// </summary>
    public IEnumerable<ProfessionItem> Items { get; set; } = new List<ProfessionItem>();

    /// <summary>
    /// Total number of items
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Profession item
    /// </summary>
    public class ProfessionItem
    {
        /// <summary>
        /// Profession ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Profession name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
} 