namespace CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;

public class GithubHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ExternalServiceHealthCheck> logger) : ExternalServiceHealthCheck(httpClientFactory, logger)
{
    protected override string ServiceName => "Github";
    protected override string HealthCheckEndpoint => "https://www.githubstatus.com/api/v2/status.json";
    protected override string ApiKey => null;
    protected override string ApiKeyValue => null;
    protected override string ResponseBody => string.Empty;
    protected override bool HeaderAuthentication => false;
    protected override HttpMethod HttpMethod => HttpMethod.Get;
}
