namespace CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;

public class TwilioHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ExternalServiceHealthCheck> logger) : ExternalServiceHealthCheck(httpClientFactory, logger)
{
    protected override string ServiceName => "Twilio";
    protected override string HealthCheckEndpoint => "https://status.twilio.com/api/v2/status.json";
    protected override string ApiKey => null;
    protected override string ApiKeyValue => null;
    protected override string ResponseBody => string.Empty;
    protected override bool HeaderAuthentication => false;
    protected override HttpMethod HttpMethod => HttpMethod.Get;
}
