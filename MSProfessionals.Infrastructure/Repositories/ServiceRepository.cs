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
/// Implementation of Service repository
/// </summary>
public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRepository"/> class.
    /// </summary>
    /// <param name="context">Database context</param>
    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Service>> GetAllAsync(int skip, int take, string? name = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Services.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var normalizedName = name.RemoveAccents().ToLower();
            var services = await query.ToListAsync(cancellationToken);
            return services
                .Where(s => s.Name.RemoveAccents().ToLower().Contains(normalizedName))
                .OrderBy(s => s.Name)
                .Skip(skip)
                .Take(take);
        }

        return await query
            .OrderBy(s => s.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await _context.Services.CountAsync(cancellationToken);
        }

        var normalizedName = name.RemoveAccents().ToLower();
        var services = await _context.Services.ToListAsync(cancellationToken);
        return services.Count(s => s.Name.RemoveAccents().ToLower().Contains(normalizedName));
    }

    /// <inheritdoc />
    public async Task<Service?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.RemoveAccents().ToLower();
        var services = await _context.Services.ToListAsync(cancellationToken);
        return services.FirstOrDefault(s => s.Name.RemoveAccents().ToLower() == normalizedName);
    }

    /// <inheritdoc />
    public async Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        await _context.Services.AddAsync(service, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Service service, CancellationToken cancellationToken = default)
    {
        _context.Services.Update(service);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Service service, CancellationToken cancellationToken = default)
    {
        _context.Services.Remove(service);
        await _context.SaveChangesAsync(cancellationToken);
    }
} 