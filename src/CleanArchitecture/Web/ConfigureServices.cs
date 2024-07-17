using System.Diagnostics;
using System.Reflection;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Web.Extensions;
using CleanArchitecture.Web.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CleanArchitecture.Web;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appsettings)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddAuthentication();
        services.AddAuthorization();

        // Middleware
        services.AddSingleton<GlobalExceptionMiddleware>();
        services.AddSingleton<PerformanceMiddleware>();
        services.AddSingleton<Stopwatch>();

        // Extension classes
        services.AddHealthChecks();
        services.AddCompressionCustom();
        services.AddCorsCustom(appsettings);
        services.AddHttpClient();
        services.AddSwaggerCustom();
        services.AddJWTCustom(appsettings);
        services.SetupHealthCheck(appsettings);

        return services;
    }
}
