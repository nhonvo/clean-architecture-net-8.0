using Microsoft.OpenApi.Models;

namespace Clean.Architecture.Web.Extensions
{
    public static class SwaggerExtension
    {
        private static readonly string[] value = new[] { "Bearer" };

        public static IServiceCollection AddSwaggerCustom(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Template",
                    Version = "v1",
                    Description = "API template project",
                    Contact = new OpenApiContact
                    {
                        Url = new Uri("https://google.com")
                    }
                });

                // Add JWT authentication support in Swagger
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

                var securityRequirement = new OpenApiSecurityRequirement
                {
            {
                securityScheme, value }
                };

                options.AddSecurityRequirement(securityRequirement);
            });
            return services;
        }
    }
}
