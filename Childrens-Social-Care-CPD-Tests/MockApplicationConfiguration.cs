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
    public bool _disableSecureCookies = false;
    public int _featurePollingInterval = 0;
    public string _gitHash = null;
    public string _googleTagManagerKey = null;
    public string _searchApiKey = null;
    public string _searchEndpoint = null;
    public string _searchIndexName = null;
    public string _azureDataProtectionContainerName = null;
    public string _azureManagedIdentityId = null;
    public string _azureStorageAccount = null;
    public string _azureStorageAccountUriFormatString = null;

    public string AppInsightsConnectionString => _appInsightsConnectionString;
    public string AppVersion => _appVersion;
    public string AzureEnvironment => _azureEnvironment;
    public string ClarityProjectId => _clarityProjectId;
    public string ContentfulDeliveryApiKey => _contentfulDeliveryApiKey;
    public string ContentfulEnvironment => _contentfulEnvironment;
    public string ContentfulGraphqlConnectionString => _contentfulGraphqlConnectionString;
    public string ContentfulPreviewHost => _contentfulPreviewHost;
    public string ContentfulPreviewId => _contentfulPreviewId;
    public string ContentfulSpaceId => _contentfulSpaceId;
    public bool DisableSecureCookies => _disableSecureCookies;
    public int FeaturePollingInterval => _featurePollingInterval;
    public string GitHash => _gitHash;
    public string GoogleTagManagerKey => _googleTagManagerKey;
    public string SearchApiKey => _searchApiKey;
    public string SearchEndpoint => _searchEndpoint;
    public string SearchIndexName => _searchIndexName;

    public string AzureDataProtectionContainerName => _azureDataProtectionContainerName;
    public string AzureManagedIdentityId => _azureManagedIdentityId;
    public string AzureStorageAccount => _azureStorageAccount;
    public string AzureStorageAccountUriFormatString => _azureStorageAccountUriFormatString;

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
        _disableSecureCookies = false;
        _featurePollingInterval = 0;
        _gitHash = value;
        _googleTagManagerKey = value;
        _searchApiKey = value;
        _searchEndpoint = value;
        _searchIndexName = value;
        _azureDataProtectionContainerName = value;
        _azureManagedIdentityId = value;
        _azureStorageAccount = value;
        _azureStorageAccountUriFormatString = value;
    }
}
