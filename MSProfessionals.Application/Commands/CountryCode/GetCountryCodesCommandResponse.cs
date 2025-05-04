using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.CountryCode;

/// <summary>
/// Response for the GetCountryCodesCommand
/// </summary>
public class GetCountryCodesCommandResponse
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
    /// Gets the list of country codes
    /// </summary>
    public IEnumerable<CountryCodeItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the GetCountryCodesCommandResponse
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="items">List of country codes</param>
    public GetCountryCodesCommandResponse(int pageNumber, int pageSize, int totalItems, IEnumerable<CountryCodeItem> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Items = items;
    }
}

/// <summary>
/// Represents a country code item in the paginated list
/// </summary>
public class CountryCodeItem
{
    /// <summary>
    /// Gets the country code ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the country code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the country name
    /// </summary>
    public string CountryName { get; }

    /// <summary>
    /// Initializes a new instance of the CountryCodeItem
    /// </summary>
    /// <param name="id">Country code ID</param>
    /// <param name="code">Country code</param>
    /// <param name="countryName">Country name</param>
    public CountryCodeItem(Guid id, string code, string countryName)
    {
        Id = id;
        Code = code;
        CountryName = countryName;
    }
} 