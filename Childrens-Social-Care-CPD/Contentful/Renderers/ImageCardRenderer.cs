using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class QuoteRenderer : IRenderer<Quote>
{
    private readonly IRenderer<Paragraph> _paragraphRenderer;

    public QuoteRenderer(IRenderer<Paragraph> paragraphRenderer)
    {
        _paragraphRenderer = paragraphRenderer;
    }

    public IHtmlContent Render(Quote item)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-inset-text");

        foreach (var content in item.Content)
        {
            var paragraph = content as Paragraph;
            if (paragraph != null)
            {
                div.InnerHtml.AppendHtml(_paragraphRenderer.Render(paragraph));
            }
        }

        return div;
    }
}
