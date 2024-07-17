using CleanArchitecture.Application.Common;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.Web.Extensions;

public static class SwaggerExtension
{
    private static readonly string[] Value = ["Bearer"];

    public static IServiceCollection AddSwaggerOpenAPI(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("OpenAPISpecification",
            new OpenApiInfo
            {
                Title = appSettings.ApplicationDetail.ApplicationName,
                Version = "v1",
                Description = appSettings.ApplicationDetail.Description,
                Contact = new OpenApiContact
                {
                    Email = "vothuongtruongnhon2002@gmail.com",
                    Name = "Truong Nhon",
                    Url = new Uri(appSettings.ApplicationDetail.ContactWebsite),
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, Value } };

            options.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
}
