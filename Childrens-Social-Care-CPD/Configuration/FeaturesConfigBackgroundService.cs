namespace Childrens_Social_Care_CPD.Configuration;

public class FeaturesConfigBackgroundService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IFeaturesConfigUpdater _featureConfigurationUpdater;

    public FeaturesConfigBackgroundService(ILogger<FeaturesConfigBackgroundService> logger, IApplicationConfiguration applicationConfiguration, IFeaturesConfigUpdater featureConfigurationUpdater)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(applicationConfiguration);
        ArgumentNullException.ThrowIfNull(featureConfigurationUpdater);

        _logger = logger;
        _applicationConfiguration = applicationConfiguration;
        _featureConfigurationUpdater = featureConfigurationUpdater;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background polling task started");
        stoppingToken.Register(() => _logger.LogInformation("Background polling task started"));

        if (_applicationConfiguration.FeaturePollingInterval == 0) return;

        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_applicationConfiguration.FeaturePollingInterval));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Polling at: {utcNow}", DateTime.UtcNow.ToShortTimeString());
            await _featureConfigurationUpdater.UpdateFeaturesAsync(stoppingToken);
        }   
    }
}