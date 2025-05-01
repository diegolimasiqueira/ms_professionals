using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSProfessionals.Infrastructure.Configurations;
using MSProfessionals.Infrastructure.Context;

namespace MSProfessionals.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection("DatabaseSettings").Bind(databaseSettings);
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(databaseSettings.GetConnectionString(), 
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
            });

            return services;
        }
    }
} 