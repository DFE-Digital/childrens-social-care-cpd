using Childrens_Social_Care_CPD.Configuration;

namespace Childrens_Social_Care_CPD_Tests;

public class MockApplicationConfiguration : IApplicationConfiguration
{
    public string _appInsightsConnectionString = null;
    public string _appVersion = null;
    public string _azureEnvironment = null;
    public string _clarityProjectId = null;
    public string _contentfulDeliveryApiKey = null;
    public string _contentfulEnvironment = null;
    public string _contentfulGraphqlConnectionString = null;
    public string _contentfulPreviewHost = null;
    public string _contentfulPreviewId = null;
    public string _contentfulSpaceId = null;
    public string _disableSecureCookies = null;
    public string _featurePollingInterval = null;
    public string _gitHash = null;
    public string _googleTagManagerKey = null;
    public string _searchApiKey = null;
    public string _searchEndpoint = null;
    public string _searchIndexName = null;

    public IConfigurationSetting<string> AppInsightsConnectionString => new StringConfigSetting(() => _appInsightsConnectionString);
    public IConfigurationSetting<string> AppVersion => new StringConfigSetting(() => _appVersion);
    public IConfigurationSetting<string> AzureEnvironment => new StringConfigSetting(() => _azureEnvironment);
    public IConfigurationSetting<string> ClarityProjectId => new StringConfigSetting(() => _clarityProjectId);
    public IConfigurationSetting<string> ContentfulDeliveryApiKey => new StringConfigSetting(() => _contentfulDeliveryApiKey);
    public IConfigurationSetting<string> ContentfulEnvironment => new StringConfigSetting(() => _contentfulEnvironment);
    public IConfigurationSetting<string> ContentfulGraphqlConnectionString => new StringConfigSetting(() => _contentfulGraphqlConnectionString);
    public IConfigurationSetting<string> ContentfulPreviewHost => new StringConfigSetting(() => _contentfulPreviewHost);
    public IConfigurationSetting<string> ContentfulPreviewId => new StringConfigSetting(() => _contentfulPreviewId);
    public IConfigurationSetting<string> ContentfulSpaceId => new StringConfigSetting(() => _contentfulSpaceId);
    public IConfigurationSetting<bool> DisableSecureCookies => new BooleanConfigSetting(() => _disableSecureCookies);
    public IConfigurationSetting<int> FeaturePollingInterval => new IntegerConfigSetting(() => _featurePollingInterval);
    public IConfigurationSetting<string> GitHash => new StringConfigSetting(() => _gitHash);
    public IConfigurationSetting<string> GoogleTagManagerKey => new StringConfigSetting(() => _googleTagManagerKey);
    public IConfigurationSetting<string> SearchApiKey => new StringConfigSetting(() => _searchApiKey);
    public IConfigurationSetting<string> SearchEndpoint => new StringConfigSetting(() => _searchEndpoint);
    public IConfigurationSetting<string> SearchIndexName => new StringConfigSetting(() => _searchIndexName);

    public void SetAllValid(string value = "foo")
    {
        _appInsightsConnectionString = value;
        _appVersion = value;
        _azureEnvironment = value;
        _clarityProjectId = value;
        _contentfulDeliveryApiKey = value;
        _contentfulEnvironment = value;
        _contentfulGraphqlConnectionString = value;
        _contentfulPreviewHost = value;
        _contentfulPreviewId = value;
        _contentfulSpaceId = value;
        _disableSecureCookies = "false";
        _featurePollingInterval = "0";
        _gitHash = value;
        _googleTagManagerKey = value;
        _searchApiKey = value;
        _searchEndpoint = value;
        _searchIndexName = value;
    }
}
