namespace MSProfessionals.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware.ExceptionMiddleware>();
        }
    }
} 