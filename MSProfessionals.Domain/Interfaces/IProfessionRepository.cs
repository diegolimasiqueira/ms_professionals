using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for the profession repository
/// </summary>
public interface IProfessionRepository
{
    /// <summary>
    /// Gets a profession by ID
    /// </summary>
    /// <param name="id">Profession ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The profession if found</returns>
    Task<Profession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professions
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professions</returns>
    Task<IEnumerable<Profession>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professions with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professions</returns>
    Task<IEnumerable<Profession>> GetAllAsync(int skip, int take, string? name = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of professions
    /// </summary>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of professions</returns>
    Task<int> CountAsync(string? name = null, CancellationToken cancellationToken = default);
} 