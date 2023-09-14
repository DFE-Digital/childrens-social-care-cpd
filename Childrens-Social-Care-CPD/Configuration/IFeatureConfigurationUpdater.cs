namespace Childrens_Social_Care_CPD.Configuration;

public interface IFeatureConfigurationUpdater
{
    Task UpdateFeaturesAsync(CancellationToken cancellationToken);
}
