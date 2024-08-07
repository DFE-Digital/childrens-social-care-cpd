using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Configuration;

[ExcludeFromCodeCoverage]
public static class ContentfulConfiguration
{
    public static bool IsPreviewEnabled(IApplicationConfiguration applicationConfiguration)
    {
        string azureEnvironment = applicationConfiguration.AzureEnvironment,
            contentfulEnvironment = applicationConfiguration.ContentfulEnvironment;

        // return true if azure environment is pre-prod
        if (string.Equals(azureEnvironment, ApplicationEnvironment.PreProduction, StringComparison.OrdinalIgnoreCase)) return true;

        // return true if azure environment is dev and contentful environment is test
        if (string.Equals(azureEnvironment, ApplicationEnvironment.Development, StringComparison.OrdinalIgnoreCase)
            && string.Equals(contentfulEnvironment, ApplicationEnvironment.Test, StringComparison.OrdinalIgnoreCase)) return true;

        // for all other cases return false
        return false;
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