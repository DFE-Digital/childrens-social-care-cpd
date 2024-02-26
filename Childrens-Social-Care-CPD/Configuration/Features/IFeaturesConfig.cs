namespace Childrens_Social_Care_CPD.Configuration.Features;

public interface IFeaturesConfig
{
    void AddOrUpdateFeature(string featureName, bool enabled);
    bool IsEnabled(string featureName);
}
