using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web.Middlewares;

namespace CleanArchitecture.Web.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
    {
        builder.Services.AddInfrastructuresService(appsettings);
        builder.Services.AddApplicationService(appsettings);
        builder.Services.AddWebAPIService(appsettings);

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { });
        using var scope = app.Services.CreateScope();

        if (!appsettings.UseInMemoryDatabase)
        {
            var initialize = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
            await initialize.InitializeAsync();
        }

        app.UseMiddleware<GlobalExceptionMiddleware>(); // Global exception handling first
        app.UseResponseCompression();  // Compression should be high up

        app.UseSwagger();
        app.UseSwaggerUI(setupAction =>
        {
            setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Clean Architecture Specification");
            setupAction.RoutePrefix = "swagger";
        });

        app.UseCors("AllowSpecificOrigin");

        app.UseHttpsRedirection(); // Ensure HTTPS redirection for security

        app.UseMiddleware<PerformanceMiddleware>(); // Performance middleware after other setup steps

        app.ConfigureHealthCheck(); // Health checks

        app.AddEndpoints(); // Add custom endpoints (if any)

        app.UseAuthentication(); // Authentication before authorization
        app.UseMiddleware<LoggingMiddleware>(); // Logging middleware after authentication to log authenticated requests

        app.ConfigureExceptionHandler(loggerFactory.CreateLogger("Exceptions")); // Global exception handler

        app.UseAuthorization(); // Authorization after authentication

        app.MapControllers(); // Map controllers after authentication and authorization

        return app;
    }

}
