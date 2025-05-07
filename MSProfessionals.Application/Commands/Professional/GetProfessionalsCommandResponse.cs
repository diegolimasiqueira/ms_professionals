namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Response for the GetProfessionalsCommand
/// </summary>
public class GetProfessionalsCommandResponse
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
    /// Gets the list of professionals
    /// </summary>
    public IEnumerable<ProfessionalItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of the GetProfessionalsCommandResponse
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="items">List of professionals</param>
    public GetProfessionalsCommandResponse(int pageNumber, int pageSize, int totalItems, IEnumerable<ProfessionalItem> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Items = items;
    }
}

/// <summary>
/// Represents a professional item in the paginated list
/// </summary>
public class ProfessionalItem
{
    /// <summary>
    /// Gets the professional ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the professional name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the professional document ID
    /// </summary>
    public string DocumentId { get; }

    /// <summary>
    /// Gets the professional photo URL
    /// </summary>
    public string? PhotoUrl { get; }

    /// <summary>
    /// Gets the professional phone number
    /// </summary>
    public string PhoneNumber { get; }

    /// <summary>
    /// Gets the professional email
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the professional social media links
    /// </summary>
    public Dictionary<string, string>? SocialMedia { get; }

    /// <summary>
    /// Gets the professional media links
    /// </summary>
    public Dictionary<string, string>? Media { get; }

    /// <summary>
    /// Gets the professional currency ID
    /// </summary>
    public Guid CurrencyId { get; }

    /// <summary>
    /// Gets the professional phone country code ID
    /// </summary>
    public Guid PhoneCountryCodeId { get; }

    /// <summary>
    /// Gets the professional preferred language ID
    /// </summary>
    public Guid PreferredLanguageId { get; }

    /// <summary>
    /// Gets the professional timezone ID
    /// </summary>
    public Guid TimezoneId { get; }

    /// <summary>
    /// Gets the professional creation date
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the professional last update date
    /// </summary>
    public DateTime? UpdatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the ProfessionalItem
    /// </summary>
    /// <param name="id">Professional ID</param>
    /// <param name="name">Professional name</param>
    /// <param name="documentId">Professional document ID</param>
    /// <param name="photoUrl">Professional photo URL</param>
    /// <param name="phoneNumber">Professional phone number</param>
    /// <param name="email">Professional email</param>
    /// <param name="socialMedia">Professional social media links</param>
    /// <param name="media">Professional media links</param>
    /// <param name="currencyId">Professional currency ID</param>
    /// <param name="phoneCountryCodeId">Professional phone country code ID</param>
    /// <param name="preferredLanguageId">Professional preferred language ID</param>
    /// <param name="timezoneId">Professional timezone ID</param>
    /// <param name="createdAt">Professional creation date</param>
    /// <param name="updatedAt">Professional last update date</param>
    public ProfessionalItem(
        Guid id,
        string name,
        string documentId,
        string? photoUrl,
        string phoneNumber,
        string email,
        Dictionary<string, string>? socialMedia,
        Dictionary<string, string>? media,
        Guid currencyId,
        Guid phoneCountryCodeId,
        Guid preferredLanguageId,
        Guid timezoneId,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        DocumentId = documentId;
        PhotoUrl = photoUrl;
        PhoneNumber = phoneNumber;
        Email = email;
        SocialMedia = socialMedia;
        Media = media;
        CurrencyId = currencyId;
        PhoneCountryCodeId = phoneCountryCodeId;
        PreferredLanguageId = preferredLanguageId;
        TimezoneId = timezoneId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
} 