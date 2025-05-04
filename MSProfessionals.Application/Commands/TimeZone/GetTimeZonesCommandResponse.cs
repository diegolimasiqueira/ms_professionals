using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.TimeZone;

/// <summary>
/// Response for the GetTimeZonesCommand
/// </summary>
public class GetTimeZonesCommandResponse
{
    /// <summary>
    /// Gets the current page number
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the page size
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the total number of items
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// Gets the list of time zones
    /// </summary>
    public IEnumerable<TimeZoneItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the GetTimeZonesCommandResponse
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="items">List of time zones</param>
    public GetTimeZonesCommandResponse(int pageNumber, int pageSize, int totalItems, IEnumerable<TimeZoneItem> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Items = items;
    }
}

/// <summary>
/// Represents a time zone item in the paginated list
/// </summary>
public class TimeZoneItem
{
    /// <summary>
    /// Gets the time zone ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the time zone code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the time zone description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the TimeZoneItem
    /// </summary>
    /// <param name="id">Time zone ID</param>
    /// <param name="code">Time zone code</param>
    /// <param name="description">Time zone description</param>
    public TimeZoneItem(Guid id, string code, string description)
    {
        Id = id;
        Code = code;
        Description = description;
    }
} 