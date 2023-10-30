namespace Childrens_Social_Care_CPD.Configuration;

public interface IConfigurationSetting
{
    bool IsSet { get; }
}

public interface IConfigurationSetting<out T> : IConfigurationSetting
{
    T Value { get; }
}

public class ConfigurationSetting<T> : IConfigurationSetting<T>
{
    private readonly Func<string> _valueGetter;
    private readonly Func<string, T> _valueParser;
    private readonly T _defaultValue;

    public ConfigurationSetting(Func<string> valueGetter, Func<string, T> valueParser, T defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(valueGetter);
        ArgumentNullException.ThrowIfNull(valueParser);

        _valueGetter = valueGetter;
        _valueParser = valueParser;
        _defaultValue = defaultValue;
    }

    private static bool CheckIfSet(string value) => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));

    public T Value
    {
        get
        {
            var value = _valueGetter();
            return CheckIfSet(value) ? _valueParser(value) : _defaultValue;
        }
    }

    public bool IsSet => CheckIfSet(_valueGetter());

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}

internal class BooleanConfigSetting : ConfigurationSetting<bool>
{
    public BooleanConfigSetting(Func<string> valueGetter, bool defaultValue = false) : base(valueGetter, bool.Parse, defaultValue)
    { }
}

internal class StringConfigSetting : ConfigurationSetting<string>
{
    public StringConfigSetting(Func<string> valueGetter, string defaultValue = "") : base(valueGetter, x => x, defaultValue)
    { }
}

internal class IntegerConfigSetting : ConfigurationSetting<int>
{
    public IntegerConfigSetting(Func<string> valueGetter, int defaultValue = 0) : base(valueGetter, int.Parse, defaultValue)
    { }
}