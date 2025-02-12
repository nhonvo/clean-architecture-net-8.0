using CleanArchitecture.Application.Common;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Web.Extensions;
/// <summary>
/// Provides extension methods for configuring health checks
/// </summary>
public static class HealthCheckExtensions
{
    public static void SetupHealthCheck(this IServiceCollection services, AppSettings configuration)
    {
        // Add health checks
        var healthCheckBuilder = services.AddHealthChecks();

        // Add database health check
        healthCheckBuilder.AddSqlServer(
            connectionString: configuration.ConnectionStrings.DefaultConnection,
            name: HealthCheck.DBHealthCheck,
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { HealthCheck.InfrastructureCheck }
        );
        if (configuration.EnableExternalHealthCheck)
        {
            // Add external service health checks
            healthCheckBuilder.AddCheck<GithubHealthCheck>(
                name: nameof(GithubHealthCheck),
                tags: new[] { HealthCheck.ExternalServiceCheck }
            ).AddCheck<TwilioHealthCheck>(
                name: nameof(TwilioHealthCheck),
                tags: new[] { HealthCheck.ExternalServiceCheck }
            ).AddCheck<OpenAPIHealthCheck>(
                name: nameof(OpenAPIHealthCheck),
                tags: new[] { HealthCheck.ExternalServiceCheck }
            );
        }

        // Configure Health Check UI
        services.AddHealthChecksUI(setup =>
            setup.AddHealthCheckEndpoint(
                "Application Health", configuration.AppUrl + "healthz"))
            .AddInMemoryStorage();
    }

    /// <summary>
    /// Configures health checks for the application
    /// </summary>
    /// <param name="app"></param>
    public static void ConfigureHealthCheck(this WebApplication app)
    {
        // Health check endpoint (basic and detailed combined)
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        duration = entry.Value.Duration
                    }),
                    totalDuration = report.TotalDuration
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });

        app.UseHealthChecks("/synthetic-check", new HealthCheckOptions
        {
            Predicate = check =>
            check.Tags.Contains(HealthCheck.InfrastructureCheck) ||
            check.Tags.Contains(HealthCheck.ExternalServiceCheck)
        });

        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI(setup =>
        {
            setup.ApiPath = "/health-ui-api";
            setup.UIPath = "/health-ui";
        });
    }
}
