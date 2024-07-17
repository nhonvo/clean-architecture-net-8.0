using System.Diagnostics;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Web;
using CleanArchitecture.Web.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Unittest.Web;

public class DependencyInjectionTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly AppSettings _appSettings = new()
    {
        ApplicationDetail = new ApplicationDetail
        {
            ApplicationName = "app",
            ContactWebsite = "http://dummy.html",
            Description = "description",
            LicenseDetail = "dummy"
        },
        ConnectionStrings = new ConnectionStrings
        {
            DefaultConnection = "dummy"
        }
    };

    public DependencyInjectionTests()
    {
        var service = new ServiceCollection();
        service.AddApplicationService();
        service.AddInfrastructuresService(_appSettings);
        service.AddWebAPIService(_appSettings);

        _serviceProvider = service.BuildServiceProvider();
    }

    [Fact]
    public void DependencyInjectionTests_ServiceShouldResolveCorrectly()
    {
        var currentTimeServiceResolved = _serviceProvider.GetRequiredService<ICurrentTime>();
        var exceptionMiddlewareResolved = _serviceProvider.GetRequiredService<GlobalExceptionMiddleware>();
        var performanceMiddleware = _serviceProvider.GetRequiredService<PerformanceMiddleware>();
        var stopwatchResolved = _serviceProvider.GetRequiredService<Stopwatch>();

        Assert.Equal(typeof(CurrentTime), currentTimeServiceResolved.GetType());
        Assert.Equal(typeof(GlobalExceptionMiddleware), exceptionMiddlewareResolved.GetType());
        Assert.Equal(typeof(Stopwatch), stopwatchResolved.GetType());
        Assert.Equal(typeof(PerformanceMiddleware), performanceMiddleware.GetType());
    }
}
