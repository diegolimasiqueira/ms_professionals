using System;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace MSProfessionals.Application;

/// <summary>
/// Extension methods for dependency injection
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.Lifetime = ServiceLifetime.Scoped;
        });
        return services;
    }
} 