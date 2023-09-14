using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Extensions;
using Contentful.Core.Search;

namespace Childrens_Social_Care_CPD.Configuration;

public class FeatureConfigurationUpdater : IFeatureConfigurationUpdater
{
    private readonly ILogger _logger;
    private readonly ICpdContentfulClient _client;
    private readonly IFeaturesConfiguration _featuresConfiguration;

    public FeatureConfigurationUpdater(ILogger<FeatureConfigurationUpdater> logger, ICpdContentfulClient client, IFeaturesConfiguration featuresConfiguration)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(featuresConfiguration, nameof(featuresConfiguration));

        _logger = logger;
        _client = client;
        _featuresConfiguration = featuresConfiguration;
    }

    public async Task UpdateFeaturesAsync(CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<ApplicationFeatures>.New
            .ContentTypeIs("applicationFeatures")
            .FieldEquals("fields.id", "config")
            .Include(2);

        try
        {
            _logger.LogInformation("Refreshing feature configuration");
            var response = await _client.GetEntries(queryBuilder, cancellationToken);

            var result = response?.FirstOrDefault();
            if (result == null || result.Features == null)
            {
                _logger.LogInformation("No configuration found");
                return;
            }

            _logger.LogInformation("Fetched configuration: {0}", result.ConvertObjectToJsonString());
            foreach (var feature in result.Features)
            {
                _featuresConfiguration.AddOrUpdateFeature(feature.Name, feature.IsEnabled);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception querying for feature configuration. Does the FeatureConfiguration model exist in Contentful?");
        }
    }

}
