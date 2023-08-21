﻿namespace Childrens_Social_Care_CPD;

public interface IApplicationConfiguration 
{ 
    string AppInsightsConnectionString { get; }
    string AzureEnvironment { get; }
    string ClarityProjectId { get; }
    string ContentfulDeliveryApiKey { get; }
    string ContentfulEnvironment { get; }
    string ContentfulPreviewHost { get; }
    string ContentfulPreviewId { get; }
    string ContentfulSpaceId { get; }
    bool DisableSecureCookies { get; }
    string GitHash { get; }
    string GoogleTagManagerKey { get; }
}