using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for ProfessionalAddress repository
/// </summary>
public interface IProfessionalAddressRepository
{
    /// <summary>
    /// Gets a professional address by ID
    /// </summary>
    /// <param name="id">Professional address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The professional address if found, null otherwise</returns>
    Task<ProfessionalAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professional addresses for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional addresses</returns>
    Task<IEnumerable<ProfessionalAddress>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the default professional address for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The default professional address if found, null otherwise</returns>
    Task<ProfessionalAddress?> GetDefaultByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new professional address
    /// </summary>
    /// <param name="professionalAddress">Professional address to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing professional address
    /// </summary>
    /// <param name="professionalAddress">Professional address to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a professional address
    /// </summary>
    /// <param name="professionalAddress">Professional address to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default);
} 