using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for ProfessionalService repository
/// </summary>
public interface IProfessionalServiceRepository
{
    /// <summary>
    /// Gets a professional service by ID
    /// </summary>
    /// <param name="id">Professional service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The professional service if found, null otherwise</returns>
    Task<ProfessionalService?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professional services for a professional profession
    /// </summary>
    /// <param name="professionalProfessionId">Professional profession ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional services</returns>
    Task<IEnumerable<ProfessionalService>> GetByProfessionalProfessionIdAsync(Guid professionalProfessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professional services for a service
    /// </summary>
    /// <param name="serviceId">Service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional services</returns>
    Task<IEnumerable<ProfessionalService>> GetByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a professional service by professional profession ID and service ID
    /// </summary>
    /// <param name="professionalProfessionId">Professional profession ID</param>
    /// <param name="serviceId">Service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The professional service if found, null otherwise</returns>
    Task<ProfessionalService?> GetByProfessionalProfessionIdAndServiceIdAsync(Guid professionalProfessionId, Guid serviceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new professional service
    /// </summary>
    /// <param name="professionalService">Professional service to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing professional service
    /// </summary>
    /// <param name="professionalService">Professional service to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a professional service
    /// </summary>
    /// <param name="professionalService">Professional service to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default);
} 