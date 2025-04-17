namespace Childrens_Social_Care_CPD.Configuration;

public class ApplicationConfiguration(IConfiguration configuration) : IApplicationConfiguration
{
    public string AppInsightsConnectionString => configuration["CPD_INSTRUMENTATION_CONNECTIONSTRING"];
    public string AppVersion => configuration["VCS-TAG"];
    public string AzureEnvironment => configuration["CPD_AZURE_ENVIRONMENT"];
    public string ClarityProjectId => configuration["CPD_CLARITY"];
    public string ContentfulDeliveryApiKey => configuration["CPD_DELIVERY_KEY"];
    public string ContentfulEnvironment => configuration["CPD_CONTENTFUL_ENVIRONMENT"];
    public string ContentfulPreviewHost => "preview.contentful.com";
    public string ContentfulPreviewId => configuration["CPD_PREVIEW_KEY"];
    public bool ContentfulForcePreview => bool.TryParse(configuration["CPD_CONTENTFUL_FORCE_PREVIEW"], out var result) && result;
    public string ContentfulSpaceId => configuration["CPD_SPACE_ID"];
    public bool DisableSecureCookies => bool.TryParse(configuration["CPD_DISABLE_SECURE_COOKIES"], out var result) && result;
    public int FeaturePollingInterval => int.TryParse(configuration["CPD_FEATURE_POLLING_INTERVAL"], out var result) ? result : 0;
    public string GitHash => configuration["VCS-REF"];
    public string GoogleTagManagerKey => configuration["CPD_GOOGLEANALYTICSTAG"];
    public string SearchApiKey => configuration["CPD_SEARCH_CLIENT_API_KEY"];
    public string SearchEndpoint => configuration["CPD_SEARCH_ENDPOINT"];
    public string SearchIndexName => configuration["CPD_SEARCH_INDEX_NAME"];
}