using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MSProfessionals.Application.Extensions
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
} 