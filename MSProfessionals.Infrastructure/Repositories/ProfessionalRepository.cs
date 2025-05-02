using System;
using System.Collections.Generic;
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
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Professional?> GetByIdAsync(Guid id)
    {
        return await _context.Professionals
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Professional>> GetAllAsync()
    {
        return await _context.Professionals.ToListAsync();
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
} 