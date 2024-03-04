namespace Childrens_Social_Care_CPD.Configuration.Features;

public interface IFeaturesConfigUpdater
{
    Task UpdateFeaturesAsync(CancellationToken cancellationToken);
}
