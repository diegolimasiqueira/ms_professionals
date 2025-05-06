using MSProfessionals.API.Middleware;

namespace MSProfessionals.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
} 