using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Context;

/// <summary>
/// Interface for the application database context
/// </summary>
public interface IApplicationDbContext
{
    DbSet<CountryCode> CountryCodes { get; set; }
    DbSet<Currency> Currencies { get; set; }
    DbSet<Language> Languages { get; set; }
    DbSet<ProfessionalAddress> ProfessionalAddresses { get; set; }
    DbSet<Professional> Professionals { get; set; }
    DbSet<Domain.Entities.TimeZone> TimeZones { get; set; }
    DbSet<Profession> Professions { get; set; }
    DbSet<Service> Services { get; set; }
    DbSet<ProfessionalProfession> ProfessionalProfessions { get; set; }
    DbSet<ProfessionalService> ProfessionalServices { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}