using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

#nullable enable

namespace Childrens_Social_Care_CPD;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}