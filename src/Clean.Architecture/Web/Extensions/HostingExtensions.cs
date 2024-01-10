using Clean.Architecture.Application;
using Clean.Architecture.Application.Common;
using Clean.Architecture.Infrastructure;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Middlewares;

namespace Clean.Architecture.Web.Extensions
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings configuration)
        {
            builder.Services.AddInfrastructuresService(configuration);
            builder.Services.AddApplicationService();
            builder.Services.AddWebAPIService(configuration);

            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings configuration)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                
            });
            using var scope = app.Services.CreateScope();
            if (!configuration.UseInMemoryDatabase)
            {
                var initialize = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                // await initialize.InitializeAsync();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("_myAllowSpecificOrigins");

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseMiddleware<PerformanceMiddleware>();

            app.UseResponseCompression();

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.ConfigureHealthCheck();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.ConfigureExceptionHandler(loggerFactory.CreateLogger("Exceptions"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
