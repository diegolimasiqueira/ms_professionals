using System.ComponentModel.DataAnnotations;

namespace MSProfessionals.Application.Commands.Currency;

/// <summary>
/// Response for the GetCurrenciesCommand
/// </summary>
public class GetCurrenciesCommandResponse
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
    /// Gets the list of currencies
    /// </summary>
    public IEnumerable<CurrencyItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the GetCurrenciesCommandResponse
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="items">List of currencies</param>
    public GetCurrenciesCommandResponse(int pageNumber, int pageSize, int totalItems, IEnumerable<CurrencyItem> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Items = items;
    }
}

/// <summary>
/// Represents a currency item in the paginated list
/// </summary>
public class CurrencyItem
{
    /// <summary>
    /// Gets the currency ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the currency code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the currency description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the CurrencyItem
    /// </summary>
    /// <param name="id">Currency ID</param>
    /// <param name="code">Currency code</param>
    /// <param name="description">Currency description</param>
    public CurrencyItem(Guid id, string code, string description)
    {
        if (code == null)
            throw new ArgumentNullException(nameof(code));
        if (description == null)
            throw new ArgumentNullException(nameof(description));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty or whitespace", nameof(code));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty or whitespace", nameof(description));

        Id = id;
        Code = code;
        Description = description;
    }
} 