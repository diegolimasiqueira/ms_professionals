using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MSProfessionals.Application.Extensions;

/// <summary>
/// Extension methods for MediatR
/// </summary>
public static class MediatRExtensions
{
    /// <summary>
    /// Adds MediatR to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="assembly">The assembly to scan for handlers</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        return services;
    }
} 