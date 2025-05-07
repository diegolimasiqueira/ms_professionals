using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Professional profession repository implementation
/// </summary>
public class ProfessionalProfessionRepository : IProfessionalProfessionRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProfessionalProfessionRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public ProfessionalProfessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ProfessionalProfession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Include(pp => pp.Professional)
            .Include(pp => pp.Profession)
            .FirstOrDefaultAsync(pp => pp.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalProfession>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Include(pp => pp.Professional)
            .Include(pp => pp.Profession)
            .Where(pp => pp.ProfessionalId == professionalId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalProfession>> GetByProfessionIdAsync(Guid professionId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Include(pp => pp.Professional)
            .Include(pp => pp.Profession)
            .Where(pp => pp.ProfessionId == professionId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProfessionalProfession?> GetMainByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalProfessions
            .Include(pp => pp.Professional)
            .Include(pp => pp.Profession)
            .FirstOrDefaultAsync(pp => pp.ProfessionalId == professionalId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalProfessions.AddAsync(professionalProfession, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalProfessions.Update(professionalProfession);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(ProfessionalProfession professionalProfession, CancellationToken cancellationToken = default)
    {
        _context.ProfessionalProfessions.Remove(professionalProfession);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
} 