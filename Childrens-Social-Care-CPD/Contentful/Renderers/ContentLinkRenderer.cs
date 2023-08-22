using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class ContentLinkRenderer : IRenderer<ContentLink>
{
    public IHtmlContent Render(ContentLink item)
    {
        var tagBuilder = new TagBuilder("a");

        var href = item.Uri switch
        {
            string s when s.StartsWith("http") => s,
            string s when s.StartsWith('/') => s,
            _ => string.Format("/{0}", item.Uri)
        };

        tagBuilder.Attributes.Add("href", href);
        tagBuilder.AddCssClass("govuk-link");
        tagBuilder.InnerHtml.Append(item.Name);
        return tagBuilder;
    }
}
