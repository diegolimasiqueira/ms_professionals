using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementation of ProfessionalService repository
/// </summary>
public class ProfessionalServiceRepository : IProfessionalServiceRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessionalServiceRepository"/> class.
    /// </summary>
    /// <param name="context">Database context</param>
    public ProfessionalServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ProfessionalService?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalServices
            .Include(ps => ps.ProfessionalProfession)
            .Include(ps => ps.Service)
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalService>> GetByProfessionalProfessionIdAsync(Guid professionalProfessionId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalServices
            .Include(ps => ps.ProfessionalProfession)
            .Include(ps => ps.Service)
            .Where(ps => ps.ProfessionalProfessionId == professionalProfessionId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalService>> GetByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalServices
            .Include(ps => ps.ProfessionalProfession)
            .Include(ps => ps.Service)
            .Where(ps => ps.ServiceId == serviceId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProfessionalService?> GetByProfessionalProfessionIdAndServiceIdAsync(Guid professionalProfessionId, Guid serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalServices
            .Include(ps => ps.ProfessionalProfession)
            .Include(ps => ps.Service)
            .FirstOrDefaultAsync(ps => ps.ProfessionalProfessionId == professionalProfessionId && ps.ServiceId == serviceId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalServices.AddAsync(professionalService, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalServices.Update(professionalService);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(ProfessionalService professionalService, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalServices.Remove(professionalService);
        await _context.SaveChangesAsync(cancellationToken);
    }
} 