using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Extensions;
using Contentful.Core.Search;

namespace Childrens_Social_Care_CPD.Configuration;

public class FeatureConfigurationPoller : BackgroundService
{
    private readonly ILogger _logger;
    private readonly ICpdContentfulClient _client;
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IFeaturesConfiguration _featuresConfiguration;

    public FeatureConfigurationPoller(ILogger<FeatureConfigurationPoller> logger, ICpdContentfulClient client, IApplicationConfiguration applicationConfiguration, IFeaturesConfiguration featuresConfiguration)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(applicationConfiguration, nameof(applicationConfiguration));
        ArgumentNullException.ThrowIfNull(featuresConfiguration, nameof(featuresConfiguration));

        _logger = logger;
        _client = client;
        _applicationConfiguration = applicationConfiguration;
        _featuresConfiguration = featuresConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Feature configuration poller started");

        cancellationToken.Register(() => _logger.LogInformation("Feature configuration poller stopping"));

        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_applicationConfiguration.FeaturePollingInterval));
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                _logger.LogInformation($"Feature configuration polling at {DateTime.UtcNow.ToShortTimeString()}");
                await QueryForFeatures(cancellationToken);
            }
        }
        finally
        {
            _logger.LogInformation("Feature configuration poller exiting");
        }
    }

    private async Task QueryForFeatures(CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<ApplicationFeatures>.New
            .ContentTypeIs("applicationFeatures")
            .FieldEquals("fields.id", "config")
            .Include(2);

        try
        {
            var response = await _client.GetEntries(queryBuilder, cancellationToken);

            var result = response?.FirstOrDefault();
            if (result == null || result.Features == null)
            {
                _logger.LogInformation("Feature Poller: could not find feature configuration");
                return;
            }

            _logger.LogInformation("Features Poller: retrieved configuration: {0}", result.ConvertObjectToJsonString());
            foreach (var feature in result.Features)
            {
                _featuresConfiguration.AddOrUpdateFeature(feature.Name, feature.IsEnabled);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Features Poller: exception querying for feature configuration. Does the FeatureConfiguration model exist in Contentful?");
        }
    }
}