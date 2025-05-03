using System;
using System.Threading;
using System.Threading.Tasks;
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
} 