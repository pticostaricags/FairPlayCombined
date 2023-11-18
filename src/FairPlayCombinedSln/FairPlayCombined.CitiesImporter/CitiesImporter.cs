namespace FairPlayCombined.CitiesImporter;

public class CitiesImporter : BackgroundService
{
    private readonly ILogger<CitiesImporter> _logger;

    public CitiesImporter(ILogger<CitiesImporter> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
        _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
    }
}
