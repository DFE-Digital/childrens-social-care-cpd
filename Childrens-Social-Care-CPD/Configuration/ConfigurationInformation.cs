using System.Collections.ObjectModel;

namespace Childrens_Social_Care_CPD.Configuration;

public record ConfigurationItemInfo(string Name, bool Required, bool Obfuscated, bool Hidden, bool IsSet, string Value, bool Extraneous);

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
            var isSet = IsSet(value);
            var displayValue = GetDisplayValue(rule, isSet, value);

            // Don't add extraneous values that haven't been set
            if (rule == null && !isSet) continue;

            list.Add(new ConfigurationItemInfo(
                Name: property.Name,
                Required: rule != null,
                Obfuscated: rule?.Obfuscate ?? true,
                Hidden: rule?.Hidden ?? false,
                IsSet: isSet,
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

    private static bool IsSet(object value)
    {
        if (value == null)
        {
            return false;
        }

        if (value is string)
        {
            var v = value as string;
            return !(string.IsNullOrEmpty(v) || string.IsNullOrWhiteSpace(v));
        }

        return true;
    }
}