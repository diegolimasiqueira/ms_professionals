using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the Professional repository
/// </summary>
public class ProfessionalRepository : IProfessionalRepository
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProfessionalRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public ProfessionalRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Professional?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Professional?> GetByIdAsync(Guid id)
    {
        return await _context.Professionals
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Professional?> GetByNameAsync(string name)
    {
        return await _context.Professionals
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Professional>> GetAllAsync()
    {
        return await _context.Professionals
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Professional professional, CancellationToken cancellationToken = default)
    {
        await _context.Professionals.AddAsync(professional, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Professional professional)
    {
        _context.Professionals.Update(professional);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var professional = await GetByIdAsync(id);
        if (professional != null)
        {
            _context.Professionals.Remove(professional);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<Professional?> GetAsync(Expression<Func<Professional, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.ProfessionalProfessions)
                .ThenInclude(pp => pp.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    /// <summary>
    /// Gets a profession by name
    /// </summary>
    /// <param name="name">Profession name</param>
    /// <returns>The found profession or null</returns>
    public async Task<Profession?> GetProfessionByNameAsync(string name)
    {
        return await _context.Professions
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <summary>
    /// Gets a service by name
    /// </summary>
    /// <param name="name">Service name</param>
    /// <returns>The found service or null</returns>
    public async Task<Service?> GetServiceByNameAsync(string name)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    /// <summary>
    /// Adds a new professional profession
    /// </summary>
    /// <param name="professionalProfession">Professional profession to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    public async Task AddProfessionalProfessionAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalProfessions.AddAsync(professionalProfession, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new professional service
    /// </summary>
    /// <param name="professionalService">Professional service to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    public async Task AddProfessionalServiceAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalServices.AddAsync(professionalService, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}