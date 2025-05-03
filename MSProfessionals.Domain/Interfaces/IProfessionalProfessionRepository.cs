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
    /// <returns>The professional profession if found</returns>
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
    /// Gets the main profession for a professional
    /// </summary>
    /// <param name="professionalId">Professional ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The main profession if found</returns>
    Task<ProfessionalProfession?> GetMainByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a professional profession
    /// </summary>
    /// <param name="professionalProfession">Professional profession to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a professional profession
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

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 