namespace Childrens_Social_Care_CPD.Configuration;

public interface IFeaturesConfiguration
{
    void AddOrUpdateFeature(string featureName, bool enabled);
    bool IsEnabled(string featureName);
}
