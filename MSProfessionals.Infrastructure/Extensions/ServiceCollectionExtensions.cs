using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSProfessionals.Infrastructure.Configurations;
using MSProfessionals.Infrastructure.Context;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Repositories;
using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            if (databaseSettings == null)
                throw new InvalidOperationException("DatabaseSettings section is missing in configuration");
            // Configure DatabaseSettings
           services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                databaseSettings.GetConnectionString(),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Register repositories
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IAddressRepository, ProfessionalAddressRepository>();
            services.AddScoped<IProfessionalServiceRepository, ProfessionalServiceRepository>();
            services.AddScoped<IProfessionalProfessionRepository, ProfessionalProfessionRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();                 
            services.AddScoped<ICountryCodeRepository, CountryCodeRepository>();       
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICountryCodeRepository, CountryCodeRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ITimeZoneRepository, TimeZoneRepository>();  
           
            return services;
        }
    }
}