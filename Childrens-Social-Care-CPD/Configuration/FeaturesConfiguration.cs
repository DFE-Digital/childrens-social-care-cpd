using Childrens_Social_Care_CPD.Contentful.Models;
using System.Collections.Concurrent;

namespace Childrens_Social_Care_CPD.Configuration;

public sealed class FeaturesConfiguration : IFeaturesConfiguration
{
    private static readonly ConcurrentDictionary<string, bool> _features = new ();

    public static void UpdateFeatureConfig(ApplicationFeatures applicationFeatures)
    {
        ArgumentNullException.ThrowIfNull(applicationFeatures, nameof(applicationFeatures));

        if (applicationFeatures.Features == null)
        {
            return;
        }

        foreach (var feature in applicationFeatures.Features)
        {
            _features.AddOrUpdate(feature.Name, feature.IsEnabled, (k, v) => feature.IsEnabled);
        }
    }

    public bool IsEnabled(string featureName)
    {
        return _features.ContainsKey(featureName) && _features[featureName];
    }
}
