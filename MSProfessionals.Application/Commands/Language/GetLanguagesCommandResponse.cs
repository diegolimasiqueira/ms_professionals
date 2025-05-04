using System;
using System.Collections.Generic;

namespace MSProfessionals.Application.Commands.Language;

/// <summary>
/// Response for the GetLanguagesCommand
/// </summary>
public class GetLanguagesCommandResponse
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
    /// Gets the list of languages
    /// </summary>
    public IEnumerable<LanguageItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the GetLanguagesCommandResponse
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="items">List of languages</param>
    public GetLanguagesCommandResponse(int pageNumber, int pageSize, int totalItems, IEnumerable<LanguageItem> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Items = items;
    }
}

/// <summary>
/// Represents a language item in the paginated list
/// </summary>
public class LanguageItem
{
    /// <summary>
    /// Gets the language ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the language code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the language description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the LanguageItem
    /// </summary>
    /// <param name="id">Language ID</param>
    /// <param name="code">Language code</param>
    /// <param name="description">Language description</param>
    public LanguageItem(Guid id, string code, string description)
    {
        Id = id;
        Code = code;
        Description = description;
    }
} 