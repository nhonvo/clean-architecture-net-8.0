using System.Diagnostics;
using System.Reflection;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Domain.Authorization;
using CleanArchitecture.Web.Extensions;
using CleanArchitecture.Web.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Web;

public static class ConfigureServices
{
        public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appSettings)
        {
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddFluentValidationAutoValidation();
                services.AddFluentValidationClientsideAdapters();
                services.AddAuth(appSettings.Jwt);
                services.AddDistributedMemoryCache();
                services.AddMemoryCache();
                services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

                // Middleware
                services.AddSingleton<GlobalExceptionMiddleware>();
                services.AddSingleton<PerformanceMiddleware>();
                services.AddSingleton<Stopwatch>();

                // Extension classes
                services.AddHealthChecks();
                services.AddCompressionCustom();
                services.AddCorsCustom(appSettings);
                services.AddHttpClient();
                services.AddSwaggerOpenAPI(appSettings);
                services.SetupHealthCheck(appSettings);

                return services;
        }
}
