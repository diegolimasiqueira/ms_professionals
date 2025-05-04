using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for Service repository
/// </summary>
public interface IServiceRepository
{
    /// <summary>
    /// Gets a service by ID
    /// </summary>
    /// <param name="id">Service ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The service if found, null otherwise</returns>
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all services
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of services</returns>
    Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all services with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of services</returns>
    Task<IEnumerable<Service>> GetAllAsync(int skip, int take, string? name = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of services
    /// </summary>
    /// <param name="name">Optional name filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of services</returns>
    Task<int> CountAsync(string? name = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a service by name
    /// </summary>
    /// <param name="name">Service name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The service if found, null otherwise</returns>
    Task<Service?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new service
    /// </summary>
    /// <param name="service">Service to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(Service service, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing service
    /// </summary>
    /// <param name="service">Service to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(Service service, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a service
    /// </summary>
    /// <param name="service">Service to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(Service service, CancellationToken cancellationToken = default);
} 