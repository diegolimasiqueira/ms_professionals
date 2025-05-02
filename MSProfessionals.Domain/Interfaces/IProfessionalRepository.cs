using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;
using System.Threading;

namespace MSProfessionals.Domain.Interfaces
{
    /// <summary>
    /// Interface for the Professional repository
    /// </summary>
    public interface IProfessionalRepository
    {
        /// <summary>
        /// Gets a professional by email
        /// </summary>
        /// <param name="email">Professional email</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a professional by ID
        /// </summary>
        /// <param name="id">Professional ID</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all professionals
        /// </summary>
        /// <returns>A collection of professionals</returns>
        Task<IEnumerable<Professional>> GetAllAsync();

        /// <summary>
        /// Adds a new professional
        /// </summary>
        /// <param name="professional">Professional to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task AddAsync(Professional professional, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing professional
        /// </summary>
        /// <param name="professional">Professional to update</param>
        /// <returns>Task</returns>
        Task UpdateAsync(Professional professional);

        /// <summary>
        /// Deletes a professional by ID
        /// </summary>
        /// <param name="id">Professional ID</param>
        /// <returns>Task</returns>
        Task DeleteAsync(Guid id);
    }
} 