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
            setup.AddHealthCheckEndpoint("Basic Health Check", "/health/full"))
                .AddInMemoryStorage();
    }

    public static void ConfigureHealthCheck(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Health");
            }
        });

        // Custom health check response writer
        app.UseHealthChecks("/health/full", new HealthCheckOptions
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

        app.UseHealthChecksUI(setup =>
        {
            setup.ApiPath = "/health/api";
            setup.UIPath = "/health/dashboard";
        });
    }
}
