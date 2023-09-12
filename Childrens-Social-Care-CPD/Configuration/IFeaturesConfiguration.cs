namespace Childrens_Social_Care_CPD.Configuration;

public interface IFeaturesConfiguration
{
    bool IsEnabled(string featureName);
}
