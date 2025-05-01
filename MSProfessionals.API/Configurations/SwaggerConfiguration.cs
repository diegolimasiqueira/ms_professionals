using Microsoft.OpenApi.Models;

namespace MSProfessionals.API.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MSProfessionals API",
                    Version = "v1",
                    Description = "API for managing professionals",
                    Contact = new OpenApiContact
                    {
                        Name = "MSProfessionals Team",
                        Email = "contact@msprofessionals.com"
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSProfessionals API v1");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
} 