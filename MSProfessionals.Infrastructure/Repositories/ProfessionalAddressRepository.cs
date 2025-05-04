using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Infrastructure.Context;
using MSProfessionals.Domain.Interfaces;

namespace MSProfessionals.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de endereços
/// </summary>
public class ProfessionalAddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância do AddressRepository
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public ProfessionalAddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task AddAsync(ProfessionalAddress professionalAddress)
    {
        await _context.ProfessionalAddresses.AddAsync(professionalAddress);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ProfessionalAddress professionalAddress)
    {
        _context.Entry(professionalAddress).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(ProfessionalAddress professionalAddress)
    {
        _context.ProfessionalAddresses.Remove(professionalAddress);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<ProfessionalAddress?> GetByIdAsync(Guid id)
    {
        return await _context.ProfessionalAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfessionalAddress>> GetByProfessionalIdAsync(Guid professionalId)
    {
        return await _context.ProfessionalAddresses
            .AsNoTracking()
            .Where(a => a.ProfessionalId == professionalId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProfessionalAddress?> GetDefaultByProfessionalIdAsync(Guid professionalId)
    {
        return await _context.ProfessionalAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ProfessionalId == professionalId && a.IsDefault);
    }
} 