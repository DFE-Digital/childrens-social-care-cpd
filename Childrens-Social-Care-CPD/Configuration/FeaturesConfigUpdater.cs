using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Extensions;
using Contentful.Core.Search;

namespace Childrens_Social_Care_CPD.Configuration;

public class FeaturesConfigUpdater : IFeaturesConfigUpdater
{
    private readonly ILogger _logger;
    private readonly ICpdContentfulClient _client;
    private readonly IFeaturesConfig _featuresConfiguration;

    public FeaturesConfigUpdater(ILogger<FeaturesConfigUpdater> logger, ICpdContentfulClient client, IFeaturesConfig featuresConfiguration)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(featuresConfiguration);

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

            _logger.LogInformation("Fetched configuration: {json}", result.ConvertObjectToJsonString());
            foreach (var feature in result.Features)
            {
                _featuresConfiguration.AddOrUpdateFeature(feature.Name, feature.IsEnabled);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception querying for feature configuration. Does the ApplicationFeatures model exist in Contentful?");
        }
    }

}
