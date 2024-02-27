using Childrens_Social_Care_CPD.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Childrens_Social_Care_CPD;

public class ConfigurationHealthCheck(ILogger<ConfigurationHealthCheck> logger, IApplicationConfiguration applicationConfiguration) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var configurationInformation = new ConfigurationInformation(applicationConfiguration);
        var healthy = true;
        
        foreach (var item in configurationInformation.ConfigurationInfo)
        {
            if (item.Required && !item.IsSet)
            {
                logger.LogError("Configuration setting {propertyName} does not have a value", item.Name);
                healthy = false;
            }
        }

        // Specific check as this is super important.
        if (applicationConfiguration.DisableSecureCookies)
        {
            logger.LogError("DisableSecureCookies should not be enabled for standard environments");
            healthy = false;
        }

        if (!healthy)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("One or more application configuration settings is missing or incorrect"));
        }
        
        return Task.FromResult(HealthCheckResult.Healthy("Application configuration is OK"));
    }
}