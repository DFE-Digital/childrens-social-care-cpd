namespace Childrens_Social_Care_CPD.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration
{
    private readonly IConfiguration _configuration;

    public ApplicationConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AppInsightsConnectionString => _configuration["CPD_INSTRUMENTATION_CONNECTIONSTRING"];
    public string AppVersion => _configuration["VCS-TAG"];
    public string AzureDataProtectionContainerName => _configuration["CPD_AZURE_DATA_PROTECTION_CONTAINER_NAME"];
    public string AzureEnvironment => _configuration["CPD_AZURE_ENVIRONMENT"];
    public string AzureManagedIdentityId => _configuration["CPD_AZURE_MANAGED_IDENTITY_ID"];
    public string AzureStorageAccount => _configuration["CPD_AZURE_STORAGE_ACCOUNT"];
    public string AzureStorageAccountUriFormatString => _configuration["CPD_AZURE_STORAGE_ACCOUNT_URI_FORMAT_STRING"];
    public string ClarityProjectId => _configuration["CPD_CLARITY"];
    public string ContentfulDeliveryApiKey => _configuration["CPD_DELIVERY_KEY"];
    public string ContentfulEnvironment => _configuration["CPD_CONTENTFUL_ENVIRONMENT"];
    public string ContentfulGraphqlConnectionString => $"https://graphql.contentful.com/content/v1/spaces/{ContentfulSpaceId}/environments/{ContentfulEnvironment}";
    public string ContentfulPreviewHost => "preview.contentful.com";
    public string ContentfulPreviewId => _configuration["CPD_PREVIEW_KEY"];
    public string ContentfulSpaceId => _configuration["CPD_SPACE_ID"];
    public bool DisableSecureCookies => bool.TryParse(_configuration["CPD_DISABLE_SECURE_COOKIES"], out var result) && result;
    public int FeaturePollingInterval => int.TryParse(_configuration["CPD_FEATURE_POLLING_INTERVAL"], out var result) ? result : 0;
    public string GitHash => _configuration["VCS-REF"];
    public string GoogleTagManagerKey => _configuration["CPD_GOOGLEANALYTICSTAG"];
    public string SearchApiKey => _configuration["CPD_SEARCH_CLIENT_API_KEY"];
    public string SearchEndpoint => _configuration["CPD_SEARCH_ENDPOINT"];
    public string SearchIndexName => _configuration["CPD_SEARCH_INDEX_NAME"];
}