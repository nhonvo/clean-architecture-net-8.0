using System.Text;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;

public abstract class ExternalServiceHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ExternalServiceHealthCheck> logger) : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<ExternalServiceHealthCheck> _logger = logger;

    protected abstract string ServiceName { get; }
    protected abstract string HealthCheckEndpoint { get; }
    protected abstract string ApiKey { get; }
    protected abstract string ApiKeyValue { get; }
    protected abstract string ResponseBody { get; }
    protected abstract bool HeaderAuthentication { get; }
    protected abstract HttpMethod HttpMethod { get; }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ServiceName)) throw new UserFriendlyException(errorCode: ErrorCode.Internal, "ServiceName must be provided.", "ServiceName must be provided.");
        if (string.IsNullOrEmpty(HealthCheckEndpoint)) throw new UserFriendlyException(errorCode: ErrorCode.Internal, "HealthCheckEndpoint must be provided.", "HealthCheckEndpoint must be provided.");

        var httpClient = _httpClientFactory.CreateClient(ServiceName);

        try
        {
            var request = BuildHttpRequest();
            var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Health check for {ServiceName} succeeded with status code {StatusCode}.", ServiceName, response.StatusCode);
                return HealthCheckResult.Healthy($"{ServiceName} is healthy.");
            }
            else
            {
                _logger.LogWarning("Health check for {ServiceName} failed with status code {StatusCode}.", ServiceName, response.StatusCode);
                return HealthCheckResult.Unhealthy($"{ServiceName} is unhealthy. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the health check for {ServiceName}.", ServiceName);
            return HealthCheckResult.Unhealthy($"{ServiceName} health check failed. Exception: {ex.Message}");
        }
    }

    private HttpRequestMessage BuildHttpRequest()
    {
        var request = new HttpRequestMessage(HttpMethod, HealthCheckEndpoint);

        if (HeaderAuthentication && !string.IsNullOrEmpty(ApiKey))
        {
            request.Headers.Add(ApiKey, ApiKeyValue);
        }

        if (HttpMethod == HttpMethod.Post && !string.IsNullOrEmpty(ResponseBody))
        {
            request.Content = new StringContent(ResponseBody, Encoding.UTF8, "application/json");
        }

        return request;
    }
}



