using Contentful.Core.Models.Management;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Configuration;

[ExcludeFromCodeCoverage]
public static class ContentfulConfiguration
{
    public static bool IsPreviewEnabled(IApplicationConfiguration applicationConfiguration)
    {
        var azureEnvironment = applicationConfiguration.AzureEnvironment;
        return azureEnvironment.IsSet
            && azureEnvironment.Value != ApplicationEnvironment.LoadTest
            && applicationConfiguration.ContentfulEnvironment.Value.ToLower() != azureEnvironment.Value.ToLower();
    }

    public static ConfigurationManager GetContentfulConfiguration(ConfigurationManager configuration, IApplicationConfiguration applicationConfiguration)
    {
        var contentfulEnvironment = applicationConfiguration.ContentfulEnvironment;
        configuration["ContentfulOptions:Environment"] = contentfulEnvironment.Value;
        configuration["ContentfulOptions:SpaceId"] = applicationConfiguration.ContentfulSpaceId.Value;
        configuration["ContentfulOptions:DeliveryApiKey"] = applicationConfiguration.ContentfulDeliveryApiKey.Value;

        var azureEnvironment = applicationConfiguration.AzureEnvironment;
        if (IsPreviewEnabled(applicationConfiguration))
        {
            configuration["ContentfulOptions:host"] = applicationConfiguration.ContentfulPreviewHost.Value;
            configuration["ContentfulOptions:UsePreviewApi"] = "true";
            configuration["ContentfulOptions:PreviewApiKey"] = applicationConfiguration.ContentfulPreviewId.Value;
        }
        return configuration;
    }
}