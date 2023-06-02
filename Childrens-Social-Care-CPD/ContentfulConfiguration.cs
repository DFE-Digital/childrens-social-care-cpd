using Childrens_Social_Care_CPD.Constants;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD
{
    [ExcludeFromCodeCoverage]
    public static class ContentfulConfiguration
    {
        public static ConfigurationManager GetContentfulConfiguration(ConfigurationManager configuration)
        {
            var contentfulEnvironment = Environment.GetEnvironmentVariable(SiteConstants.ENVIRONMENT) ?? String.Empty;
            var appEnvironment = Environment.GetEnvironmentVariable(SiteConstants.AZUREENVIRONMENT) ?? String.Empty;
            var deliveryApiKey = Environment.GetEnvironmentVariable(SiteConstants.DELIVERYAPIKEY) ?? String.Empty;
            var spaceId = Environment.GetEnvironmentVariable(SiteConstants.CONTENTFULSPACEID) ?? String.Empty;

            configuration["ContentfulOptions:Environment"] = contentfulEnvironment;
            configuration["ContentfulOptions:SpaceId"] = spaceId;
            configuration["ContentfulOptions:DeliveryApiKey"] = deliveryApiKey;

            if ((contentfulEnvironment.ToLower() != appEnvironment.ToLower()) && !String.IsNullOrEmpty(appEnvironment) && appEnvironment!= SiteConstants.LOADTESTAPPENVIRONMENT)
            {
                var previewApiKey = Environment.GetEnvironmentVariable(SiteConstants.PREVIEWAPIKEY) ?? String.Empty;
                configuration["ContentfulOptions:host"] = SiteConstants.CONTENTFULPREVIEWHOST;
                configuration["ContentfulOptions:UsePreviewApi"] = "true";
                configuration["ContentfulOptions:PreviewApiKey"] = previewApiKey;
            }
            return configuration;
        }
    }
}
