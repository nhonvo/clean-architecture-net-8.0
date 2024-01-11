using System.Diagnostics;
using System.Reflection;
using Clean.Architecture.Application.Common;
using Clean.Architecture.Web.Extensions;
using Clean.Architecture.Web.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Clean.Architecture.Web
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings configuration)
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
            services.AddCorsCustom();
            services.AddHttpClient();
            services.AddSwaggerCustom();
            services.AddJWTCustom(configuration);
            services.SetupHealthCheck();

            return services;
        }
    }
}
