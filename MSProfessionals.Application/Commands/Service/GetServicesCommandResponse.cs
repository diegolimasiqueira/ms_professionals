using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.Service;

/// <summary>
/// Response for the GetServicesCommand
/// </summary>
public class GetServicesCommandResponse
{
    /// <summary>
    /// List of services
    /// </summary>
    public IEnumerable<ServiceItem> Items { get; set; } = new List<ServiceItem>();

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
    /// Service item
    /// </summary>
    public class ServiceItem
    {
        /// <summary>
        /// Service ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Service name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
} 