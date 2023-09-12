namespace Childrens_Social_Care_CPD.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration
{
    private static string ValueOrStringEmpty(string key) => Environment.GetEnvironmentVariable(key) ?? string.Empty;
    public string AppInsightsConnectionString => ValueOrStringEmpty("CPD_INSTRUMENTATION_CONNECTIONSTRING");
    public string AppVersion => ValueOrStringEmpty("VCS-TAG");
    public string AzureEnvironment => ValueOrStringEmpty("CPD_AZURE_ENVIRONMENT");
    public string ClarityProjectId => ValueOrStringEmpty("CPD_CLARITY");
    public string ContentfulDeliveryApiKey => ValueOrStringEmpty("CPD_DELIVERY_KEY");
    public string ContentfulEnvironment => ValueOrStringEmpty("CPD_CONTENTFUL_ENVIRONMENT");
    public string ContentfulPreviewHost => "preview.contentful.com";
    public string ContentfulPreviewId => ValueOrStringEmpty("CPD_PREVIEW_KEY");
    public string ContentfulSpaceId => ValueOrStringEmpty("CPD_SPACE_ID");
    public bool DisableSecureCookies => ValueOrStringEmpty("CPD_DISABLE_SECURE_COOKIES") == "true";
    public int FeaturePollingInterval => int.Parse(Environment.GetEnvironmentVariable("CPD_FEATURE_POLLING_INTERVAL") ?? "60000");
    public string GitHash => ValueOrStringEmpty("VCS-REF");
    public string GoogleTagManagerKey => ValueOrStringEmpty("CPD_GOOGLEANALYTICSTAG");
}