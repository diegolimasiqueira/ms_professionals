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
/// Implementation of ProfessionalProfession repository
/// </summary>
public class ProfessionalProfessionRepository : IProfessionalProfessionRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessionalProfessionRepository"/> class.
    /// </summary>
    /// <param name="context">Database context</param>
    public ProfessionalProfessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ProfessionalProfession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalProfession>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Where(x => x.ProfessionalId == professionalId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalProfession>> GetByProfessionIdAsync(Guid professionId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Where(x => x.ProfessionId == professionId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalProfessions.AddAsync(professionalProfession, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalProfessions.Update(professionalProfession);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalProfessions.Remove(professionalProfession);
        await _context.SaveChangesAsync(cancellationToken);
    }
} 