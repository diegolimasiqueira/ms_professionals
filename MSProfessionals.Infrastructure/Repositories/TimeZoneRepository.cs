using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the ITimeZoneRepository
/// </summary>
public class TimeZoneRepository : ITimeZoneRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the TimeZoneRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public TimeZoneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Domain.Entities.TimeZone?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .FirstOrDefaultAsync(tz => tz.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.TimeZone>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.TimeZone>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Entities.TimeZone>> GetByDescriptionAsync(string description, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .AsNoTracking()
            .Where(tz => tz.Description.Contains(description))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByDescriptionAsync(string description, CancellationToken cancellationToken = default)
    {
        return await _context.TimeZones
            .Where(tz => tz.Description.Contains(description))
            .CountAsync(cancellationToken);
    }
} 