using Clean.Architecture.Application.Common.Interfaces;
using Clean.Architecture.Application.Services;

namespace Clean.Architecture.Application
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
