using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;
using MSProfessionals.Infrastructure.Extensions;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the IProfessionRepository
/// </summary>
public class ProfessionRepository : IProfessionRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProfessionRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public ProfessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Profession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Professions
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Profession>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Professions
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Profession>> GetAllAsync(int skip, int take, string? name = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Professions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var normalizedName = name.RemoveAccents().ToLower();
            var professions = await query.ToListAsync(cancellationToken);
            return professions
                .Where(p => p.Name.RemoveAccents().ToLower().Contains(normalizedName))
                .OrderBy(p => p.Name)
                .Skip(skip)
                .Take(take);
        }

        return await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await _context.Professions.CountAsync(cancellationToken);
        }

        var normalizedName = name.RemoveAccents().ToLower();
        var professions = await _context.Professions.ToListAsync(cancellationToken);
        return professions.Count(p => p.Name.RemoveAccents().ToLower().Contains(normalizedName));
    }
} 