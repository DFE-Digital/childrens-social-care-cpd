using Childrens_Social_Care_CPD.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Childrens_Social_Care_CPD;

public class ConfigurationHealthCheck : IHealthCheck
{
    private readonly ILogger _logger;
    private readonly IApplicationConfiguration _applicationConfiguration;

    public ConfigurationHealthCheck(ILogger<ConfigurationHealthCheck> logger, IApplicationConfiguration applicationConfiguration)
    {
        _logger = logger;
        _applicationConfiguration = applicationConfiguration;
    }

    private bool CheckSetting(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _logger.LogError("Configuration setting {propertyName} does not have a value", name);
            return false;
        }

        return true;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var healthy = CheckSetting("AppInsightsConnectionString", _applicationConfiguration.AppInsightsConnectionString)
            && CheckSetting("AppVersion", _applicationConfiguration.AppVersion)
            && CheckSetting("AzureEnvironment", _applicationConfiguration.AzureEnvironment)
            && CheckSetting("ClarityProjectId", _applicationConfiguration.ClarityProjectId)
            && CheckSetting("ContentfulDeliveryApiKey", _applicationConfiguration.ContentfulDeliveryApiKey)
            && CheckSetting("ContentfulEnvironment", _applicationConfiguration.ContentfulEnvironment)
            && CheckSetting("ContentfulSpaceId", _applicationConfiguration.ContentfulSpaceId)
            && CheckSetting("GoogleTagManagerKey", _applicationConfiguration.GoogleTagManagerKey);

        if (_applicationConfiguration.DisableSecureCookies)
        {
            _logger.LogError("DisableSecureCookies should not be enabled for standard environments");
            healthy = false;
        }

        if (!healthy)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("One or more application configuration settings is missing or incorrect"));
        }
        
        return Task.FromResult(HealthCheckResult.Healthy("Application configuration is OK"));
    }
}