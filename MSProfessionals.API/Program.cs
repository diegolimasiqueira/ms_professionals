using MSProfessionals.API.Extensions;
using MSProfessionals.Infrastructure.Extensions;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.API.Configurations;
using MSProfessionals.Infrastructure.Context;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Configure ForwardedHeaders
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
    options.RequireHeaderSymmetry = false;
    options.ForwardLimit = null;
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Kestrel based on environment
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5233);
    });
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80);
    });
}

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

// Add Health Checks UI
builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(5);
    setup.MaximumHistoryEntriesPerEndpoint(10);
    setup.SetApiMaxActiveRequests(1);
    setup.AddHealthCheckEndpoint("API", builder.Environment.IsDevelopment() 
        ? "http://localhost:5233/health" 
        : "http://localhost:80/health");
})
.AddInMemoryStorage();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProfessionalCommand).Assembly));

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

var app = builder.Build();

// Use ForwardedHeaders
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSProfessionals API V1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "MSProfessionals API Documentation";
        c.DefaultModelsExpandDepth(-1);
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
    });
}

// Use CORS
app.UseCors();

// Remover o redirecionamento HTTPS já que o Kong está lidando com isso
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// Add Health Check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.MapHealthChecks("/health-json", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
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
