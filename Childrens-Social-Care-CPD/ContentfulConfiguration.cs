using System.Diagnostics.CodeAnalysis;
using Childrens_Social_Care_CPD.Configuration;

namespace Childrens_Social_Care_CPD;

[ExcludeFromCodeCoverage]
public static class ContentfulConfiguration
{
    public const string LOADTESTAPPENVIRONMENT = "load-test";

    public static ConfigurationManager GetContentfulConfiguration(ConfigurationManager configuration, IApplicationConfiguration applicationConfiguration)
    {
        var contentfulEnvironment = applicationConfiguration.ContentfulEnvironment;
        configuration["ContentfulOptions:Environment"] = contentfulEnvironment;
        configuration["ContentfulOptions:SpaceId"] = applicationConfiguration.ContentfulSpaceId;
        configuration["ContentfulOptions:DeliveryApiKey"] = applicationConfiguration.ContentfulDeliveryApiKey;

        var azureEnvironment = applicationConfiguration.AzureEnvironment;
        if ((contentfulEnvironment.ToLower() != azureEnvironment.ToLower()) 
            && !String.IsNullOrEmpty(azureEnvironment) 
            && azureEnvironment!= LOADTESTAPPENVIRONMENT)
        {
            configuration["ContentfulOptions:host"] = applicationConfiguration.ContentfulPreviewHost;
            configuration["ContentfulOptions:UsePreviewApi"] = "true";
            configuration["ContentfulOptions:PreviewApiKey"] = applicationConfiguration.ContentfulPreviewId;
        }
        return configuration;
    }
}
