using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace Childrens_Social_Care_CPD;

public enum ScriptPosition
{
    HeadEnd,
    BodyStart,
    BodyEnd
}

internal record ScriptInfo(string Source, bool Async, bool Defer, ScriptPosition Position);

public static partial class CustomHtmlHelpers
{
    public static void RequireScriptUrl(this IHtmlHelper htmlHelper, string url, bool async = false, bool defer = false, ScriptPosition position = ScriptPosition.BodyEnd)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);

        if (string.IsNullOrEmpty(url)) return;

        var key = url.ToLowerInvariant();

        if (!htmlHelper.ViewContext.HttpContext.Items.TryGetValue("CustomScripts", out var items))
        {
            items = new Dictionary<string, ScriptInfo>();
        }
        var scripts = items as IDictionary<string, ScriptInfo> ?? new Dictionary<string, ScriptInfo>();
        
        if (!scripts.ContainsKey(key))
        {
            scripts.Add(url, new ScriptInfo(url, async, defer, position));
            htmlHelper.ViewContext.HttpContext.Items["CustomScripts"] = scripts;
        }
    }

    public static HtmlString RenderScripts(this IHtmlHelper htmlHelper, ScriptPosition position)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);

        if (!htmlHelper.ViewContext.HttpContext.Items.TryGetValue("CustomScripts", out var items))
        {
            items = new Dictionary<string, ScriptInfo>();
        }
        var scripts = items as IDictionary<string, ScriptInfo> ?? new Dictionary<string, ScriptInfo>();

        if (scripts.Count == 0)
        {
            return HtmlString.Empty;
        }

        var urlHelperFactory = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        var urlHelper = urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);
        var builder = new StringBuilder();
        foreach (var script in scripts.Where(s => s.Value.Position == position))
        {
            var url = urlHelper.Content(script.Value.Source);
            builder.AppendLine($"<script src=\"{url}\"{(script.Value.Defer ? " defer" : null)}{(script.Value.Async ? " async" : null)}></script>");
        }

        return new HtmlString(builder.ToString());
    }
}