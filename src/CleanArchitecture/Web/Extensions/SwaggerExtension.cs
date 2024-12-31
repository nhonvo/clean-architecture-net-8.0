using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure.SchemaFilter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.Web.Extensions;

public static class SwaggerExtension
{
    private static readonly string[] Value = ["Bearer"];

    public static IServiceCollection AddSwaggerOpenAPI(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddFluentValidationRulesToSwagger();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
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

            // Enable XML comments
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            // Add security definition for Bearer token
            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
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

            options.SchemaFilter<EnumSchemaFilter>();
            options.DocumentFilter<HealthChecksFilter>();
            options.EnableAnnotations();
        });
        return services;
    }

    public static void UseSwagger(this IApplicationBuilder app, AppSettings appSettings)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq)
                => swaggerDoc.Servers = [new OpenApiServer { Url = appSettings.AppUrl }]);
        });

        app.UseSwaggerUI(setupAction =>
        {
            setupAction.SwaggerEndpoint("v1/swagger.json", "CleanArchitecture.api v1");
            setupAction.RoutePrefix = "swagger";
        });
    }
}
