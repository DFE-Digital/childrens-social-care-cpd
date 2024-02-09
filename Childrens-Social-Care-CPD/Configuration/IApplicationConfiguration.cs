using System.ComponentModel;

namespace Childrens_Social_Care_CPD.Configuration;

public interface IApplicationConfiguration
{
    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> AppInsightsConnectionString { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> AppVersion { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> AzureDataProtectionContainerName { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> AzureEnvironment { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> AzureManagedIdentityId { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> AzureStorageAccount { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> AzureStorageAccountUriFormatString { get; }

    [RequiredForEnvironment(ApplicationEnvironment.Production, Hidden = false)]
    IConfigurationSetting<string> ClarityProjectId { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> ContentfulDeliveryApiKey { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> ContentfulEnvironment { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> ContentfulGraphqlConnectionString { get; }

    [RequiredForEnvironment(ApplicationEnvironment.PreProduction, Hidden = false)]
    IConfigurationSetting<string> ContentfulPreviewHost { get; }

    [RequiredForEnvironment(ApplicationEnvironment.PreProduction, Hidden = false)]
    IConfigurationSetting<string> ContentfulPreviewId { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> ContentfulSpaceId { get; }

    [DefaultValue(false)]
    [RequiredForEnvironment(ApplicationEnvironment.Integration, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<bool> DisableSecureCookies { get; }

    [DefaultValue(0)]
    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<int> FeaturePollingInterval { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> GitHash { get; }

    [RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> GoogleTagManagerKey { get; }

    //[RequiredForEnvironment(ApplicationEnvironment.None, Hidden = false)]
    IConfigurationSetting<string> SearchApiKey { get; }

    //[RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false)]
    IConfigurationSetting<string> SearchEndpoint { get; }

    //[RequiredForEnvironment(ApplicationEnvironment.All, Hidden = false, Obfuscate = false)]
    IConfigurationSetting<string> SearchIndexName { get; }
}
