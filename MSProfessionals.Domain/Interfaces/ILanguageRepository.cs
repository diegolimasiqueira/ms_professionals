using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for the language repository
/// </summary>
public interface ILanguageRepository
{
    /// <summary>
    /// Gets a language by ID
    /// </summary>
    /// <param name="id">Language ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The language if found</returns>
    Task<Domain.Entities.Language?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all languages
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of languages</returns>
    Task<IEnumerable<Domain.Entities.Language>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all languages with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of languages</returns>
    Task<IEnumerable<Domain.Entities.Language>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of languages
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of languages</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets languages filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered languages</returns>
    Task<IEnumerable<Domain.Entities.Language>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of languages filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of filtered languages</returns>
    Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default);
} 