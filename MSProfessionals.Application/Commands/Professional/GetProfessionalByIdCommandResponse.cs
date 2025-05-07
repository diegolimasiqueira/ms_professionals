namespace MSProfessionals.Application.Commands.Professional;

/// <summary>
/// Response for the GetProfessionalByIdCommand
/// </summary>
public class GetProfessionalByIdCommandResponse
{
    /// <summary>
    /// Professional ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Professional's name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Professional's document ID
    /// </summary>
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Professional's photo URL
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Professional's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Professional's email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Professional's social media links
    /// </summary>
    public Dictionary<string, string>? SocialMedia { get; set; }

    /// <summary>
    /// Professional's media links
    /// </summary>
    public Dictionary<string, string>? Media { get; set; }

    /// <summary>
    /// Professional's currency ID
    /// </summary>
    public Guid CurrencyId { get; set; }

    /// <summary>
    /// Professional's phone country code ID
    /// </summary>
    public Guid PhoneCountryCodeId { get; set; }

    /// <summary>
    /// Professional's preferred language ID
    /// </summary>
    public Guid PreferredLanguageId { get; set; }

    /// <summary>
    /// Professional's timezone ID
    /// </summary>
    public Guid TimezoneId { get; set; }

    /// <summary>
    /// Professional's creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Professional's last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the GetProfessionalByIdCommandResponse
    /// </summary>
    /// <param name="professional">Professional entity</param>
    public GetProfessionalByIdCommandResponse(Domain.Entities.Professional professional)
    {
        Id = professional.Id;
        Name = professional.Name;
        DocumentId = professional.DocumentId;
        PhotoUrl = professional.PhotoUrl;
        PhoneNumber = professional.PhoneNumber;
        Email = professional.Email;
        SocialMedia = professional.SocialMedia;
        Media = professional.Media;
        CurrencyId = professional.CurrencyId;
        PhoneCountryCodeId = professional.PhoneCountryCodeId;
        PreferredLanguageId = professional.PreferredLanguageId;
        TimezoneId = professional.TimezoneId;
        CreatedAt = professional.CreatedAt;
        UpdatedAt = professional.UpdatedAt;
    }
} 