using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSProfessionals.Domain.Entities;
using System.Threading;
using System.Linq.Expressions;
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
        /// Gets a professional by name
        /// </summary>
        /// <param name="name">Professional name</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetAsync(Expression<Func<Professional, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all professionals
        /// </summary>
        /// <returns>List of professionals</returns>
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

        /// <summary>
        /// Gets a profession by name
        /// </summary>
        /// <param name="name">Profession name</param>
        /// <returns>The found profession or null</returns>
        Task<Profession?> GetProfessionByNameAsync(string name);

        /// <summary>
        /// Gets a service by name
        /// </summary>
        /// <param name="name">Service name</param>
        /// <returns>The found service or null</returns>
        Task<Service?> GetServiceByNameAsync(string name);

        /// <summary>
        /// Adds a new professional profession
        /// </summary>
        /// <param name="professionalProfession">Professional profession to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task AddProfessionalProfessionAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new professional service
        /// </summary>
        /// <param name="professionalService">Professional service to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task AddProfessionalServiceAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default);
    }
} 