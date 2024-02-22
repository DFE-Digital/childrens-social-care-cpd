using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Configuration;

[ExcludeFromCodeCoverage]
public static class ContentfulConfiguration
{
    public static bool IsPreviewEnabled(IApplicationConfiguration applicationConfiguration)
    {
        var azureEnvironment = applicationConfiguration.AzureEnvironment;
        return !string.IsNullOrEmpty(azureEnvironment)
            && azureEnvironment != ApplicationEnvironment.LoadTest
            && applicationConfiguration.ContentfulEnvironment.ToLower() != azureEnvironment.ToLower();
    }

    public static ConfigurationManager GetContentfulConfiguration(ConfigurationManager configuration, IApplicationConfiguration applicationConfiguration)
    {
        var contentfulEnvironment = applicationConfiguration.ContentfulEnvironment;
        configuration["ContentfulOptions:Environment"] = contentfulEnvironment;
        configuration["ContentfulOptions:SpaceId"] = applicationConfiguration.ContentfulSpaceId;
        configuration["ContentfulOptions:DeliveryApiKey"] = applicationConfiguration.ContentfulDeliveryApiKey;

        if (IsPreviewEnabled(applicationConfiguration))
        {
            configuration["ContentfulOptions:host"] = applicationConfiguration.ContentfulPreviewHost;
            configuration["ContentfulOptions:UsePreviewApi"] = "true";
            configuration["ContentfulOptions:PreviewApiKey"] = applicationConfiguration.ContentfulPreviewId;
        }
        return configuration;
    }
}