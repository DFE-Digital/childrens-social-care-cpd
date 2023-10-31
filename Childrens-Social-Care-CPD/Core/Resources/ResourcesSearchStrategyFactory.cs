using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.DataAccess;

namespace Childrens_Social_Care_CPD.Core.Resources;

internal class ResourcesSearchStrategyFactory : IResourcesSearchStrategyFactory
{
    private readonly IFeaturesConfig _featuresConfig;
    private readonly IResourcesRepository _resourcesRepository;
    private readonly ILogger<ResourcesFixedTagsSearchStrategy> _fixedLogger;

    public ResourcesSearchStrategyFactory(IFeaturesConfig featuresConfig, IResourcesRepository resourcesRepository, ILogger<ResourcesFixedTagsSearchStrategy> fixedLogger)
    {
        _featuresConfig = featuresConfig;
        _resourcesRepository = resourcesRepository;
        _fixedLogger = fixedLogger;
    }

    public IResourcesSearchStrategy Create()
    {
        return _featuresConfig.IsEnabled(Features.ResourcesUseDynamicTags)
            ? new ResourcesDynamicTagsSearchStategy(_resourcesRepository)
            : new ResourcesFixedTagsSearchStrategy(_resourcesRepository, _fixedLogger) ;
    }
}
