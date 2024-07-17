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
        builder.Services.AddApplicationService();
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

        app.UseSwagger();
        app.UseSwaggerUI(setupAction =>
        {
            setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Clean Architecture Specification");
            setupAction.RoutePrefix = "swagger";
        });

        app.UseCors("AllowSpecificOrigin");

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseMiddleware<PerformanceMiddleware>();

        app.UseResponseCompression();

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.ConfigureHealthCheck();

        app.UseMiddleware<LoggingMiddleware>();

        app.ConfigureExceptionHandler(loggerFactory.CreateLogger("Exceptions"));

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
