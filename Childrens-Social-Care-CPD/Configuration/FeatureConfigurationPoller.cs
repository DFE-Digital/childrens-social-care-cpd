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

    public FeatureConfigurationPoller(ILogger<FeatureConfigurationPoller> logger, ICpdContentfulClient client, IApplicationConfiguration applicationConfiguration)
    {
        _logger = logger;
        _client = client;
        _applicationConfiguration = applicationConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Feature configuration poller started");

        stoppingToken.Register(() => _logger.LogInformation("Feature configuration poller stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Feature configuration polling");
            await QueryForFeatures(stoppingToken);
            await Task.Delay(_applicationConfiguration.FeaturePollingInterval, stoppingToken);
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
            if (result == null)
            {
                _logger.LogInformation("Feature Poller: could not find feature configuration");
                return;
            }

            _logger.LogInformation("Features Poller: retrieved configuration: {0}", result.ConvertObjectToJsonString());

            FeaturesConfiguration.UpdateFeatureConfig(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Features Poller: exception querying for feature configuration. Does the FeatureConfiguration model exist in Contentful?");
        }
    }
}