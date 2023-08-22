using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class HyperlinkRenderer : IRenderer<Hyperlink>
{
    public IHtmlContent Render(Hyperlink item)
    {
        var linkText = (item.Content.FirstOrDefault() as Text)?.Value;
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link");
        anchor.Attributes.Add("href", item.Data.Uri);
        anchor.InnerHtml.SetContent(linkText);
        return anchor;
    }
}
