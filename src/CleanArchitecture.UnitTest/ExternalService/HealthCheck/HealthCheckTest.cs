using System.Net;
using CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;
using CleanArchitecture.Unittest.Extension;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Unittest.ExternalService.HealthCheck;

public class HealthCheckTest
{
    [Fact]
    public async void GithubHealthCheck_CheckHealthAsync_ShouldBeHealthy()
    {
        // Given
        var logger = MockService.GetMockLogger();
        var httpClient = MockService.GetMockHttpFactory(HttpStatusCode.OK);
        var healthContext = new HealthCheckContext();
        var github = new GithubHealthCheck(httpClient, logger);
        // When
        var result = await github.CheckHealthAsync(healthContext);
        // Then
        Assert.Equal($"Github is healthy.", result.Description);
    }

    [Fact]
    public async void GithubHealthCheck_CheckHealthAsync_ShouldBeUnHealthy()
    {
        // Given
        var logger = MockService.GetMockLogger();
        var httpClient = MockService.GetMockHttpFactory(HttpStatusCode.InternalServerError);
        var healthContext = new HealthCheckContext();
        var github = new GithubHealthCheck(httpClient, logger);
        // When
        var result = await github.CheckHealthAsync(healthContext);
        // Then
        Assert.Equal($"Github is unhealthy. Status code: InternalServerError", result.Description);
    }
}
