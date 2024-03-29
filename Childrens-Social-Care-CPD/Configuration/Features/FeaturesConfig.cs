﻿using System.Collections.Concurrent;

namespace Childrens_Social_Care_CPD.Configuration.Features;

public sealed class FeaturesConfig : IFeaturesConfig
{
    private static readonly ConcurrentDictionary<string, bool> _features = new ();

    public bool IsEnabled(string featureName)
    {
        return _features.ContainsKey(featureName) && _features[featureName];
    }

    public void AddOrUpdateFeature(string featureName, bool enabled)
    {
        _features.AddOrUpdate(featureName, enabled, (k, v) => enabled);
    }
}
