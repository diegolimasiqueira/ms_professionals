using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for the country code repository
/// </summary>
public interface ICountryCodeRepository
{
    /// <summary>
    /// Gets a country code by ID
    /// </summary>
    /// <param name="id">Country code ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The country code if found</returns>
    Task<CountryCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all country codes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of country codes</returns>
    Task<IEnumerable<CountryCode>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all country codes with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of country codes</returns>
    Task<IEnumerable<CountryCode>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of country codes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of country codes</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a country code by code
    /// </summary>
    /// <param name="code">Country code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The country code if found, null otherwise</returns>
    Task<CountryCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets country codes filtered by country name
    /// </summary>
    /// <param name="countryName">Country name to filter by</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered country codes</returns>
    Task<IEnumerable<CountryCode>> GetByCountryNameAsync(string countryName, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of country codes filtered by country name
    /// </summary>
    /// <param name="countryName">Country name to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of filtered country codes</returns>
    Task<int> CountByCountryNameAsync(string countryName, CancellationToken cancellationToken = default);
} 