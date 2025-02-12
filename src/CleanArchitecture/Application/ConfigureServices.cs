using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Web.Services;

namespace CleanArchitecture.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, AppSettings appsettings)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IMediaService, MediaService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuthIdentityService, AuthIdentityService>();

        if (appsettings.FileStorageSettings.LocalStorage)
        {
            services.AddSingleton<IFileService, LocalStorageService>();
        }
        else
        {
            services.AddSingleton<IFileService, CloudinaryStorageService>();
        }


        services.AddTransient<IUserService, UserService>();

        services.AddTransient<ICurrentTime, CurrentTime>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICookieService, CookieService>();

        return services;
    }
}
