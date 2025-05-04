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
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a professional by name
        /// </summary>
        /// <param name="name">Professional name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a professional by predicate
        /// </summary>
        /// <param name="predicate">Predicate to filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetAsync(Expression<Func<Professional, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all professionals
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of professionals</returns>
        Task<IEnumerable<Professional>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all professionals with pagination
        /// </summary>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="take">Number of items to take</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of professionals</returns>
        Task<IEnumerable<Professional>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the total number of professionals
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of professionals</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

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
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UpdateAsync(Professional professional, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a professional by ID
        /// </summary>
        /// <param name="id">Professional ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a profession by name
        /// </summary>
        /// <param name="name">Profession name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found profession or null</returns>
        Task<Profession?> GetProfessionByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a service by name
        /// </summary>
        /// <param name="name">Service name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found service or null</returns>
        Task<Service?> GetServiceByNameAsync(string name, CancellationToken cancellationToken = default);

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

        /// <summary>
        /// Gets a professional by ID without loading relations
        /// </summary>
        /// <param name="id">Professional ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found professional or null</returns>
        Task<Professional?> GetByIdWithoutRelationsAsync(Guid id, CancellationToken cancellationToken = default);
    }
} 