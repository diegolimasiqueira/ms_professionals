using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MSProfessionals.Infrastructure.Configurations;
using MSProfessionals.Infrastructure.Context;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Repositories;

namespace MSProfessionals.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DatabaseSettings
            services.Configure<DatabaseSettings>(options =>
            {
                configuration.GetSection("DatabaseSettings").Bind(options);
            });
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Configure DbContext
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<DatabaseSettings>();
                options.UseNpgsql(dbSettings.GetConnectionString());
            });

            // Register repositories
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

            return services;
        }
    }
} 