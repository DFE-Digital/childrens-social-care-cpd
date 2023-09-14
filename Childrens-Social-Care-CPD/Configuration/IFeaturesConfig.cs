namespace Childrens_Social_Care_CPD.Configuration;

public interface IFeaturesConfig
{
    void AddOrUpdateFeature(string featureName, bool enabled);
    bool IsEnabled(string featureName);
}
