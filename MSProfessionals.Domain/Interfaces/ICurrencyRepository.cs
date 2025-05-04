using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for the currency repository
/// </summary>
public interface ICurrencyRepository
{
    /// <summary>
    /// Gets a currency by ID
    /// </summary>
    /// <param name="id">Currency ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The currency if found</returns>
    Task<Currency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all currencies
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of currencies</returns>
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all currencies with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of currencies</returns>
    Task<IEnumerable<Currency>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of currencies
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of currencies</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets currencies filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered currencies</returns>
    Task<IEnumerable<Currency>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of currencies filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of filtered currencies</returns>
    Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default);
} 