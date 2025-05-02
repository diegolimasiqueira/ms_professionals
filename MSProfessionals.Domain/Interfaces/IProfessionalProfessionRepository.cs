using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface for ProfessionalProfession repository
/// </summary>
public interface IProfessionalProfessionRepository
{
    /// <summary>
    /// Gets a professional profession by ID
    /// </summary>
    /// <param name="id">Professional profession ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The professional profession if found, null otherwise</returns>
    Task<ProfessionalProfession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professional professions for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional professions</returns>
    Task<IEnumerable<ProfessionalProfession>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all professional professions for a profession
    /// </summary>
    /// <param name="professionId">Profession ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of professional professions</returns>
    Task<IEnumerable<ProfessionalProfession>> GetByProfessionIdAsync(Guid professionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new professional profession
    /// </summary>
    /// <param name="professionalProfession">Professional profession to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing professional profession
    /// </summary>
    /// <param name="professionalProfession">Professional profession to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a professional profession
    /// </summary>
    /// <param name="professionalProfession">Professional profession to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default);
} 