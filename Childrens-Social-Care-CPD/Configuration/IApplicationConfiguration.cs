using System.ComponentModel;

namespace Childrens_Social_Care_CPD.Configuration;

public interface IApplicationConfiguration
{
    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    string AppInsightsConnectionString { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    string AppVersion { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    string AzureEnvironment { get; }

    [RequiredForEnvironment(ApplicationEnvironment.Production, Hidden = false)]
    string ClarityProjectId { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    string ContentfulDeliveryApiKey { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    string ContentfulEnvironment { get; }

    [RequiredForEnvironment(ApplicationEnvironment.PreProduction, Hidden = false)]
    string ContentfulPreviewHost { get; }

    [RequiredForEnvironment(ApplicationEnvironment.PreProduction, Hidden = false)]
    string ContentfulPreviewId { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    bool ContentfulForcePreview { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    string ContentfulSpaceId { get; }

    [DefaultValue(false)]
    [RequiredForEnvironment(ApplicationEnvironment.Integration, Hidden = false, Obfuscate = false)]
    bool DisableSecureCookies { get; }

    [DefaultValue(0)]
    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    int FeaturePollingInterval { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    string GitHash { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    string GoogleTagManagerKey { get; }
}
