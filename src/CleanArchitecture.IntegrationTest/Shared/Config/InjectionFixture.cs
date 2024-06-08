using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTest;
public class InjectionFixture
{
    public InjectionFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        IConfiguration config = builder.Build();

        var service = new ServiceCollection();

        var serviceProvider = service.BuildServiceProvider();
        var appSettings = serviceProvider.GetService<AppSetting>();
        service.AddSingleton(appSettings);
    }
}
