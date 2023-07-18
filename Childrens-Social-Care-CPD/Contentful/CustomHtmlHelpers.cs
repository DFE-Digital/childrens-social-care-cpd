using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful;

public static class CustomHtmlHelpers
{
    public static IHtmlContent ContentLink(this IHtmlHelper helper, string text, string uri)
    {
        if (helper == null) return null;

        var tagBuilder = new TagBuilder("a");
        
        var href = uri switch
        {
            string s when s.StartsWith("http") => s,
            string s when s.StartsWith("/") => s,
            _ => string.Format("/content/{0}", uri)
        };
        
        tagBuilder.Attributes.Add("href", href);
        tagBuilder.AddCssClass("govuk-link");
        tagBuilder.InnerHtml.Append(text);
        return tagBuilder;
    }

    public static async Task RenderContentfulPartialAsync(this IHtmlHelper helper, IContent item)
    {
        if (item == null) return;

        var partialName = PartialsFactory.GetPartialFor(item);

        if (string.IsNullOrEmpty(partialName)) return;

        await helper.RenderPartialAsync(partialName, item);
    }
}