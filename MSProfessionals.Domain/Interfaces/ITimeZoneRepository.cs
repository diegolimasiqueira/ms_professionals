using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for the time zone repository
/// </summary>
public interface ITimeZoneRepository
{
    /// <summary>
    /// Gets a time zone by ID
    /// </summary>
    /// <param name="id">Time zone ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The time zone if found</returns>
    Task<Domain.Entities.TimeZone?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all time zones
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of time zones</returns>
    Task<IEnumerable<Domain.Entities.TimeZone>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all time zones with pagination
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of time zones</returns>
    Task<IEnumerable<Domain.Entities.TimeZone>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of time zones
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of time zones</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets time zones filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered time zones</returns>
    Task<IEnumerable<Domain.Entities.TimeZone>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total number of time zones filtered by description
    /// </summary>
    /// <param name="description">Description to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of filtered time zones</returns>
    Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default);
} 