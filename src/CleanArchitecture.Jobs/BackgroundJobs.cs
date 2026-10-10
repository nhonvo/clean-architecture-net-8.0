namespace CleanArchitecture.Jobs;

public class BackgroundJobs(ILogger<BackgroundJobs> logger) : BackgroundService
{
    private readonly ILogger<BackgroundJobs> _logger = logger;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Job running at: {time}", DateTimeOffset.Now);

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
    }
}