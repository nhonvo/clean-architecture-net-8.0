using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Web.Extensions;

public static class HealthCheckExtensions
{
    public static void SetupHealthCheck(this IServiceCollection services, AppSettings configuration)
    {
        services.AddHealthChecks();
        services.AddHealthChecks()
           .AddDbContextCheck<ApplicationDbContext>(name: "Application DB Context", failureStatus: HealthStatus.Degraded)
           .AddUrlGroup(new Uri(configuration.ApplicationDetail.ContactWebsite),
                           name: configuration.ApplicationDetail.ApplicationName,
                           failureStatus: HealthStatus.Degraded)
           .AddSqlServer(configuration.ConnectionStrings.DefaultConnection);

        services.AddHealthChecksUI(setup => setup.AddHealthCheckEndpoint("Basic Health Check", $"/healthz"))
                         .AddInMemoryStorage();
    }
    public static void ConfigureHealthCheck(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                },
        }).UseHealthChecksUI(setup =>
        {
            setup.ApiPath = "/healthcheck";
            setup.UIPath = "/healthcheck-ui";
            // setup.AddCustomStylesheet("Customization\\custom.css");
        });
    }
}
