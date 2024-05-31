using System.Text;
using CleanArchitecture.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Extensions;

public static class JWTExtension
{
    public static IServiceCollection AddJWTCustom(this IServiceCollection services, AppSettings configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration.Jwt.Issuer,
            ValidAudience = configuration.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Jwt.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        });
        return services;
    }
}
