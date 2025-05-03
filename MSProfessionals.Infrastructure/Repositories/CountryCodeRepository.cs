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
/// Implementation of the CountryCode repository
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
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CountryCode>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CountryCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CountryCodes
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
} 