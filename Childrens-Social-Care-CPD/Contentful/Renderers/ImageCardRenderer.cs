using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class QuoteRenderer(IRenderer<Paragraph> paragraphRenderer) : IRenderer<Quote>
{
    public IHtmlContent Render(Quote item)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-inset-text");

        foreach (var content in item.Content)
        {
            if (content is Paragraph paragraph)
            {
                div.InnerHtml.AppendHtml(paragraphRenderer.Render(paragraph));
            }
        }

        return div;
    }
}
