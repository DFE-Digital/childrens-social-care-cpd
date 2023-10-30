namespace Childrens_Social_Care_CPD.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration
{
    private readonly IConfigurationSetting<string> _appInsightsConnectionString = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_INSTRUMENTATION_CONNECTIONSTRING"));
    private readonly IConfigurationSetting<string> _appVersion = new StringConfigSetting(() => Environment.GetEnvironmentVariable("VCS-TAG"));
    private readonly IConfigurationSetting<string> _azureEnvironment = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_AZURE_ENVIRONMENT"));
    private readonly IConfigurationSetting<string> _clarityProjectId = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_CLARITY"));
    private readonly IConfigurationSetting<string> _contentfulDeliveryApiKey = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_DELIVERY_KEY"));
    private readonly IConfigurationSetting<string> _contentfulEnvironment = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT"));
    private readonly IConfigurationSetting<string> _contentfulGraphqlConnectionString;
    private readonly IConfigurationSetting<string> _contentfulPreviewHost = new StringConfigSetting(() => "preview.contentful.com");
    private readonly IConfigurationSetting<string> _contentfulPreviewId = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_PREVIEW_KEY"));
    private readonly IConfigurationSetting<string> _contentfulSpaceId = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_SPACE_ID"));
    private readonly IConfigurationSetting<bool> _disableSecureCookies = new BooleanConfigSetting(() => Environment.GetEnvironmentVariable("CPD_DISABLE_SECURE_COOKIES"), false);
    private readonly IConfigurationSetting<int> _featurePollingInterval = new IntegerConfigSetting(() => Environment.GetEnvironmentVariable("CPD_FEATURE_POLLING_INTERVAL"), 0);
    private readonly IConfigurationSetting<string> _gitHash = new StringConfigSetting(() => Environment.GetEnvironmentVariable("VCS-REF"));
    private readonly IConfigurationSetting<string> _googleTagManagerKey = new StringConfigSetting(() => Environment.GetEnvironmentVariable("CPD_GOOGLEANALYTICSTAG"));

    public ApplicationConfiguration()
    {
        _contentfulGraphqlConnectionString = new StringConfigSetting(() => $"https://graphql.contentful.com/content/v1/spaces/{ContentfulSpaceId.Value}/environments/{ContentfulEnvironment.Value}");
    }

    public IConfigurationSetting<string> AppInsightsConnectionString => _appInsightsConnectionString;
    public IConfigurationSetting<string> AppVersion => _appVersion;
    public IConfigurationSetting<string> AzureEnvironment => _azureEnvironment;
    public IConfigurationSetting<string> ClarityProjectId => _clarityProjectId;
    public IConfigurationSetting<string> ContentfulDeliveryApiKey => _contentfulDeliveryApiKey;
    public IConfigurationSetting<string> ContentfulEnvironment => _contentfulEnvironment;
    public IConfigurationSetting<string> ContentfulGraphqlConnectionString => _contentfulGraphqlConnectionString;
    public IConfigurationSetting<string> ContentfulPreviewHost => _contentfulPreviewHost;
    public IConfigurationSetting<string> ContentfulPreviewId => _contentfulPreviewId;
    public IConfigurationSetting<string> ContentfulSpaceId => _contentfulSpaceId;
    public IConfigurationSetting<bool> DisableSecureCookies => _disableSecureCookies;
    public IConfigurationSetting<int> FeaturePollingInterval => _featurePollingInterval;
    public IConfigurationSetting<string> GitHash => _gitHash;
    public IConfigurationSetting<string> GoogleTagManagerKey => _googleTagManagerKey;
}