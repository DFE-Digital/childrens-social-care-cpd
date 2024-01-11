namespace Childrens_Social_Care_CPD.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration
{
    public ApplicationConfiguration(IConfiguration configuration)
    {
        AppInsightsConnectionString = new StringConfigSetting(() => configuration["CPD_INSTRUMENTATION_CONNECTIONSTRING"]);
        AppVersion = new StringConfigSetting(() => configuration["VCS-TAG"]);
        AzureEnvironment = new StringConfigSetting(() => configuration["CPD_AZURE_ENVIRONMENT"]);
        ClarityProjectId = new StringConfigSetting(() => configuration["CPD_CLARITY"]);
        ContentfulDeliveryApiKey = new StringConfigSetting(() => configuration["CPD_DELIVERY_KEY"]);
        ContentfulEnvironment = new StringConfigSetting(() => configuration["CPD_CONTENTFUL_ENVIRONMENT"]);
        ContentfulGraphqlConnectionString = new StringConfigSetting(() => $"https://graphql.contentful.com/content/v1/spaces/{ContentfulSpaceId.Value}/environments/{ContentfulEnvironment.Value}");
        ContentfulPreviewHost = new StringConfigSetting(() => "preview.contentful.com");
        ContentfulPreviewId = new StringConfigSetting(() => configuration["CPD_PREVIEW_KEY"]);
        ContentfulSpaceId = new StringConfigSetting(() => configuration["CPD_SPACE_ID"]);
        DisableSecureCookies = new BooleanConfigSetting(() => configuration["CPD_DISABLE_SECURE_COOKIES"], false);
        FeaturePollingInterval = new IntegerConfigSetting(() => configuration["CPD_FEATURE_POLLING_INTERVAL"], 0);
        GitHash = new StringConfigSetting(() => configuration["VCS-REF"]);
        GoogleTagManagerKey = new StringConfigSetting(() => configuration["CPD_GOOGLEANALYTICSTAG"]);
        SearchApiKey = new StringConfigSetting(() => configuration["CPD_SEARCH_CLIENT_API_KEY"]);
        SearchEndpoint = new StringConfigSetting(() => configuration["CPD_SEARCH_ENDPOINT"]);
        SearchIndexName = new StringConfigSetting(() => configuration["CPD_SEARCH_INDEX_NAME"]);
    }

    public IConfigurationSetting<string> AppInsightsConnectionString { get; init; }
    public IConfigurationSetting<string> AppVersion { get; init; }
    public IConfigurationSetting<string> AzureEnvironment { get; init; }
    public IConfigurationSetting<string> ClarityProjectId { get; init; }
    public IConfigurationSetting<string> ContentfulDeliveryApiKey { get; init; }
    public IConfigurationSetting<string> ContentfulEnvironment { get; init; }
    public IConfigurationSetting<string> ContentfulGraphqlConnectionString { get; init; }
    public IConfigurationSetting<string> ContentfulPreviewHost { get; init; }
    public IConfigurationSetting<string> ContentfulPreviewId { get; init; }
    public IConfigurationSetting<string> ContentfulSpaceId { get; init; }
    public IConfigurationSetting<bool> DisableSecureCookies { get; init; }
    public IConfigurationSetting<int> FeaturePollingInterval { get; init; }
    public IConfigurationSetting<string> GitHash { get; init; }
    public IConfigurationSetting<string> GoogleTagManagerKey { get; init; }
    public IConfigurationSetting<string> SearchApiKey { get; init; }
    public IConfigurationSetting<string> SearchEndpoint { get; init; }
    public IConfigurationSetting<string> SearchIndexName { get; init; }
}