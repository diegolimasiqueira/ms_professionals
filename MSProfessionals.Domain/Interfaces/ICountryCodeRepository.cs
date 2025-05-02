using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for CountryCode repository
/// </summary>
public interface ICountryCodeRepository
{
    /// <summary>
    /// Gets a country code by ID
    /// </summary>
    /// <param name="id">Country code ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The country code if found, null otherwise</returns>
    Task<CountryCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all country codes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of country codes</returns>
    Task<IEnumerable<CountryCode>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a country code by code
    /// </summary>
    /// <param name="code">Country code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The country code if found, null otherwise</returns>
    Task<CountryCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
} 