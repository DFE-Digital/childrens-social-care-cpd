namespace Childrens_Social_Care_CPD.Configuration;

public interface IApplicationConfiguration
{
    string AppInsightsConnectionString { get; }
    string AppVersion { get; }
    string AzureEnvironment { get; }
    string ClarityProjectId { get; }
    string ContentfulDeliveryApiKey { get; }
    string ContentfulEnvironment { get; }
    string ContentfulGraphqlConnectionString { get; }
    string ContentfulPreviewHost { get; }
    string ContentfulPreviewId { get; }
    string ContentfulSpaceId { get; }
    bool DisableSecureCookies { get; }
    int FeaturePollingInterval { get; }
    string GitHash { get; }
    string GoogleTagManagerKey { get; }
}
