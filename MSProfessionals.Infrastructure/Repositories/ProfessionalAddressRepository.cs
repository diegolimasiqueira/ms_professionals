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
/// Implementation of the professional address repository
/// </summary>
public class ProfessionalAddressRepository : IProfessionalAddressRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProfessionalAddressRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public ProfessionalAddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ProfessionalAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAddresses
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalAddress>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAddresses
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalAddress>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAddresses
            .Where(x => x.ProfessionalId == professionalId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProfessionalAddress?> GetDefaultByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAddresses
            .Include(pa => pa.Professional)
            .Include(pa => pa.Country)
            .Where(pa => pa.ProfessionalId == professionalId)
            .Where(pa => pa.IsDefault)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalAddresses.AddAsync(professionalAddress, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalAddresses.Attach(professionalAddress);
        var entry = _context.Entry(professionalAddress);
        entry.State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(ProfessionalAddress professionalAddress, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalAddresses.Remove(professionalAddress);
        await _context.SaveChangesAsync(cancellationToken);
    }
} 