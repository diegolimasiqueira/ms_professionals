using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of the ICountryCodeRepository
/// </summary>
public class CountryCodeRepository : ICountryCodeRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CountryCodeRepository
    /// </summary>
    /// <param name="context">Application database context</param>
    public CountryCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<CountryCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .FirstOrDefaultAsync(cc => cc.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CountryCode>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CountryCode>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CountryCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CountryCode>> GetByCountryNameAsync(string countryName, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .AsNoTracking()
            .Where(cc => cc.CountryName.Contains(countryName))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByCountryNameAsync(string countryName, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .Where(cc => cc.CountryName.Contains(countryName))
            .CountAsync(cancellationToken);
    }
} 