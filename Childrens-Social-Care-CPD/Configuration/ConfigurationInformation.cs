using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Childrens_Social_Care_CPD.Configuration;

public record ConfigurationItemInfo(string Name, bool Required, bool Obfuscated, bool Hidden, bool HasValue, string Value, bool Extraneous);

public class ConfigurationInformation
{
    public string Environment { get; set; }
    public ReadOnlyCollection<ConfigurationItemInfo> ConfigurationInfo { get; set; }

    public ConfigurationInformation(IApplicationConfiguration applicationConfiguration)
    {
        Environment = applicationConfiguration.AzureEnvironment;
        ExtractInfo(applicationConfiguration);
    }

    private void ExtractInfo(IApplicationConfiguration applicationConfiguration)
    {
        var properties = applicationConfiguration.RulesForEnvironment(Environment);

        var list = new List<ConfigurationItemInfo>();
        foreach (var propertyPair in properties)
        {
            var property = propertyPair.Key;
            var rule = propertyPair.Value;
            var value = property.GetValue(applicationConfiguration);
            var hasValue = HasValue(property, value);
            var displayValue = GetDisplayValue(rule, hasValue, value);

            // Don't add extraneous values that haven't been set
            if (rule == null && !hasValue) continue;

            list.Add(new ConfigurationItemInfo(
                Name: property.Name,
                Required: rule != null,
                Obfuscated: rule?.Obfuscate ?? true,
                Hidden: rule?.Hidden ?? false,
                HasValue: hasValue,
                Value: displayValue,
                Extraneous: rule == null));
        }

        ConfigurationInfo = new ReadOnlyCollection<ConfigurationItemInfo>(list);
    }

    private static string GetObfuscatedValue(bool hasValue) => hasValue ? "Set" : "Not set";

    private static string GetDisplayValue(RequiredForEnvironmentAttribute rule, bool hasValue, object value)
    {
        if (rule == null) return GetObfuscatedValue(hasValue);

        return rule.Obfuscate
            ? GetObfuscatedValue(hasValue)
            : value?.ToString();
    }

    private static bool HasValue(PropertyInfo propertyInfo, object value)
    {
        if (value == null)
        {
            return false;
        }

        if (propertyInfo.PropertyType == typeof(string))
        {
            return !string.IsNullOrEmpty(value as string);
        }

        return true;
    }
}