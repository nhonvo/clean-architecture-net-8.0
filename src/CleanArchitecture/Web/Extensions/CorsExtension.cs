namespace CleanArchitecture.Web.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsCustom(this IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(
                    policy => policy.WithOrigins("", "")
                              .AllowAnyHeader()
                              .AllowAnyMethod()));

            return services;
        }
    }
}
