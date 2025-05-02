using MSProfessionals.API.Extensions;
using MSProfessionals.Infrastructure.Extensions;
using MSProfessionals.Application.Commands;
using MSProfessionals.API.Configurations;
using MSProfessionals.Infrastructure.Context;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using MSProfessionals.API.Middleware;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Interfaces;
using MSProfessionals.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<ApplicationDbContext>(); 

// Add Health Checks UI
builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(5);
    setup.MaximumHistoryEntriesPerEndpoint(10);
    setup.SetApiMaxActiveRequests(1);
    setup.AddHealthCheckEndpoint("API", "/health");
})
.AddInMemoryStorage();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProfessionalCommand).Assembly));

// Add Infrastructure
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSProfessionals API V1");
        c.RoutePrefix = string.Empty; // Define a rota raiz para o Swagger
        c.DocumentTitle = "MSProfessionals API Documentation";
        c.DefaultModelsExpandDepth(-1); // Oculta os schemas por padrão
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// Configure Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health-json", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Add Health Checks UI
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
});

// Add global exception handler
app.UseExceptionMiddleware();

app.Run();