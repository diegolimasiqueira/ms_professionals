using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MSProfessionals.API.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MsConsumers API",
                Version = "v1",
                Description = "API for consumer management",
                Contact = new OpenApiContact
                {
                    Name = "Development Team",
                    Email = "diego@easyprofind.com",
                    Url = new Uri("https://easyprofind.com/contact")
                }               
            });

            // Include XML comments in the documentation if the file exists
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Additional configurations
            c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        });

        return services;
    }
} 
