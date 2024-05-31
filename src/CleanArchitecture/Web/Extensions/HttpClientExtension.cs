namespace CleanArchitecture.Web.Extensions;

public static class HttpClientExtension
{
    public static IServiceCollection AddHttpClientCustom(this IServiceCollection services)
    {
        services.AddHttpClient("Sample", options => options.BaseAddress = new Uri("Sample_URL"));
        return services;
    }
}
