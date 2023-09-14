namespace Childrens_Social_Care_CPD.Configuration;

public interface IFeaturesConfigUpdater
{
    Task UpdateFeaturesAsync(CancellationToken cancellationToken);
}
