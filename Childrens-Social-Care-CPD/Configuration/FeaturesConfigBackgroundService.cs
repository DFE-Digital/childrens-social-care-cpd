namespace Childrens_Social_Care_CPD.Configuration;

public class FeaturesConfigBackgroundService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IFeaturesConfigUpdater _featureConfigurationUpdater;

    public FeaturesConfigBackgroundService(ILogger<FeaturesConfigBackgroundService> logger, IApplicationConfiguration applicationConfiguration, IFeaturesConfigUpdater featureConfigurationUpdater)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(applicationConfiguration, nameof(applicationConfiguration));
        ArgumentNullException.ThrowIfNull(featureConfigurationUpdater, nameof(featureConfigurationUpdater));

        _logger = logger;
        _applicationConfiguration = applicationConfiguration;
        _featureConfigurationUpdater = featureConfigurationUpdater;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background polling task started");
        cancellationToken.Register(() => _logger.LogInformation("Background polling task started"));

        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_applicationConfiguration.FeaturePollingInterval));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            _logger.LogInformation($"Polling at: {DateTime.UtcNow.ToShortTimeString()}");
            await _featureConfigurationUpdater.UpdateFeaturesAsync(cancellationToken);
        }   
    }
}