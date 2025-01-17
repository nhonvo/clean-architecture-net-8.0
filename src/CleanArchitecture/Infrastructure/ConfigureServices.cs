using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, AppSettings configuration)
    {
        if (configuration.UseInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitecture"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection));
        }

        services.AddIdentity<ApplicationUser, RoleIdentity>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        // register services
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IBookRepository, BookRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IMediaRepository, MediaRepository>();
        services.AddTransient<IForgotPasswordRepository, ForgotPasswordRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ApplicationDbContextInitializer>();

        return services;
    }
}
