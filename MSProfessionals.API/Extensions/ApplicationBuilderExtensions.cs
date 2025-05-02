using Microsoft.AspNetCore.Builder;
using MSProfessionals.API.Middleware;

namespace MSProfessionals.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
} 