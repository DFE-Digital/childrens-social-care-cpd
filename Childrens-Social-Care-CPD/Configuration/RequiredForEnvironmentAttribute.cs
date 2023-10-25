using System.Collections.ObjectModel;
using System.Reflection;

namespace Childrens_Social_Care_CPD.Configuration;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class RequiredForEnvironmentAttribute: Attribute
{
    public string Environment { get; }
    public bool Hidden { get; set; } = true;
    public bool Obfuscate { get; set; } = true;

    public RequiredForEnvironmentAttribute(string environment)
    {
        Environment = environment;
    }
}

public static class ObjectExtensions
{
    public static ReadOnlyDictionary<PropertyInfo, RequiredForEnvironmentAttribute> RulesForEnvironment<T>(this T source, string environment)
    {
        ArgumentNullException.ThrowIfNull(source);

        var dict = new Dictionary<PropertyInfo, RequiredForEnvironmentAttribute>();
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var attributes = property.GetCustomAttributes<RequiredForEnvironmentAttribute>(true);
            if (!attributes.Any()) continue;

            var rule = RuleForCurrentEnvironment(environment, attributes);
            dict.Add(property, rule);
        }

        return new ReadOnlyDictionary<PropertyInfo, RequiredForEnvironmentAttribute>(dict);
    }

    private static RequiredForEnvironmentAttribute RuleForCurrentEnvironment(string environment, IEnumerable<RequiredForEnvironmentAttribute> attributes)
    {
        var list = new List<RequiredForEnvironmentAttribute>();
        foreach (var attribute in attributes)
        {
            if (attribute.Environment == "*")
            {
                list.Insert(0, attribute);
            }
            else if (attribute.Environment == environment)
            {
                list.Add(attribute);
            }
        }
        return list.LastOrDefault();
    }
}