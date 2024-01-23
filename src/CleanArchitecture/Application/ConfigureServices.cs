using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Services;

namespace CleanArchitecture.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBookService, BookService>();

            services.AddScoped<ISeedService, SeedService>();

            services.AddSingleton<ICurrentTime, CurrentTime>();

            return services;
        }
    }
}
