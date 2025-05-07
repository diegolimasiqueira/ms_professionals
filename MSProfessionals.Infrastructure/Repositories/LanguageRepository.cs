using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the ILanguageRepository
/// </summary>
public class LanguageRepository : ILanguageRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the LanguageRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public LanguageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Domain.Entities.Language?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.Language>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.Language>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.Language>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .AsNoTracking()
            .Where(l => l.Description.Contains(description))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default)
    {
        return await _context.Languages
            .Where(l => l.Description.Contains(description))
            .CountAsync(cancellationToken);
    }
} 