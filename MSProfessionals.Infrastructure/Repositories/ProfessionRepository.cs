using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

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
} 