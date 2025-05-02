using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Data;

public interface IApplicationDbContext
{
    DbSet<Professional> Professionals { get; set; }
    DbSet<CountryCode> CountryCodes { get; set; }
    DbSet<Currency> Currencies { get; set; }
    DbSet<Language> Languages { get; set; }
    DbSet<Domain.Entities.TimeZone> TimeZones { get; set; }
    DbSet<ProfessionalAddress> ProfessionalAddresses { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 