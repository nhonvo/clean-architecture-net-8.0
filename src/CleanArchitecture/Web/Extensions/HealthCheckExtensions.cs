using CleanArchitecture.Application.Common;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Web.Extensions;

public static class HealthCheckExtensions
{
    public static void SetupHealthCheck(this IServiceCollection services, AppSettings configuration)
    {
        services.AddHealthChecks()
                .AddSqlServer(configuration.ConnectionStrings.DefaultConnection, tags: ["local", "database"]);

        services.AddHealthChecksUI(setup =>
            setup.AddHealthCheckEndpoint("Basic Health Check", "/healthz"))
                .AddInMemoryStorage();
    }

    public static void ConfigureHealthCheck(this WebApplication app)
    {
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status429TooManyRequests,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
            AllowCachingResponses = true
        });

        // Custom health check response writer
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "text/plain"; 
                await context.Response.WriteAsync("Health"); 
            }
        });

        app.UseHealthChecksUI(setup =>
        {
            setup.ApiPath = "/healthcheck";
            setup.UIPath = "/healthcheck-ui";
        });
    }
}
