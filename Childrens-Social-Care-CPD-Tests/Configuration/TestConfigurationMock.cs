using Childrens_Social_Care_CPD.Configuration;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

/*
    * Create a mock and re-apply the attributes we want for testing as custom attributes cannot be accessed on the 
    * object outside of the assembly the class was declared in.
    * See https://learn.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/accessing-custom-attributes for more info.
    */
public class TestConfigurationMock : IApplicationConfiguration
{
    public string _appInsightsConnectionString;
    public string _appVersion;
    public string _azureEnvironment;
    public string _clarityProjectId;
    public string _contentfulDeliveryApiKey;
    public string _contentfulEnvironment;
    public string _contentfulPreviewHost;
    public string _contentfulPreviewId;
    public string _contentfulSpaceId;
    public bool _disableSecureCookies;
    public int _featurePollingInterval;
    public string _gitHash;
    public string _googleTagManagerKey;

    public string AppInsightsConnectionString => _appInsightsConnectionString;
    [RequiredForEnvironment("*", Hidden = false, Obfuscate = false)]
    public string AppVersion => _appVersion;
    [RequiredForEnvironment("*", Hidden = false, Obfuscate = false)]
    public string AzureEnvironment => _azureEnvironment;
    [RequiredForEnvironment("prod", Hidden = false, Obfuscate = true)]
    public string ClarityProjectId => _clarityProjectId;
    [RequiredForEnvironment("prod", Hidden = false, Obfuscate = true)]
    public string ContentfulDeliveryApiKey => _contentfulDeliveryApiKey;
    public string ContentfulEnvironment => _contentfulEnvironment;
    public string ContentfulPreviewHost => _contentfulPreviewHost;
    public string ContentfulPreviewId => _contentfulPreviewId;
    public string ContentfulSpaceId => _contentfulSpaceId;
    public bool DisableSecureCookies => _disableSecureCookies;
    public int FeaturePollingInterval => _featurePollingInterval;
    public string GitHash => _gitHash;
    public string GoogleTagManagerKey => _googleTagManagerKey;

    public TestConfigurationMock()
    {
        _appInsightsConnectionString = "";
        _appVersion = "";
        _azureEnvironment = "";
        _clarityProjectId = "";
        _contentfulDeliveryApiKey = "";
        _contentfulEnvironment = "";
        _contentfulPreviewHost = "";
        _contentfulPreviewId = "";
        _contentfulSpaceId = "";
        _disableSecureCookies = false;
        _featurePollingInterval = 0;
        _gitHash = "";
        _googleTagManagerKey = "";
    }
}