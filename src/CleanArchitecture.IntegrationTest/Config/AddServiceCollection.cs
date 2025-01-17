using CleanArchitecture.IntegrationTest.Shared.Client;
using CleanArchitecture.IntegrationTest.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTest.Config;

public class AddServiceCollection
{
    public readonly IServiceProvider _serviceProvider;
    public AddServiceCollection()
    {
        var serviceCollection = new ServiceCollection();
        // register service here
        serviceCollection.AddTransient<IBookClient, BookClient>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

    }
}
