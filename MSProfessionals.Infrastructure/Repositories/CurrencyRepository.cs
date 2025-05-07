using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the ICurrencyRepository
/// </summary>
public class CurrencyRepository : ICurrencyRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CurrencyRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public CurrencyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Currency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Currency>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Currency>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .AsNoTracking()
            .Where(c => c.Description.Contains(description))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .Where(c => c.Description.Contains(description))
            .CountAsync(cancellationToken);
    }
} 