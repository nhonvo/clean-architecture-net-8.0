using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Utilities;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Web.Services;

namespace CleanArchitecture.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IFacebookAuthService, FacebookAuthService>();
        services.AddScoped<IManageMediaService, ManageMediaService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthIdentityService, AuthIdentityService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICurrentTime, CurrentTime>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICookieService, CookieService>();

        return services;
    }
}
