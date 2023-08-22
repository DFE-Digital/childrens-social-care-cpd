using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TableHeaderRenderer : IRenderer<TableHeader>
{
    private readonly IRenderer<Text> _textRenderer;
    private readonly IRenderer<Hyperlink> _hyperlinkRenderer;

    public TableHeaderRenderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer)
    {
        _textRenderer = textRenderer;
        _hyperlinkRenderer = hyperlinkRenderer;
    }

    public IHtmlContent Render(TableHeader item)
    {
        var th = new TagBuilder("th");
        th.AddCssClass("govuk-table__header");
        th.Attributes.Add("scope", "col");

        foreach (var content in item.Content)
        {
            switch (content)
            {
                case Paragraph paragraph:
                    {
                        foreach (var pContent in paragraph.Content)
                        {
                            switch (pContent)
                            {
                                case Text text: th.InnerHtml.AppendHtml(_textRenderer.Render(text)); break;
                                case Hyperlink hyperlink: th.InnerHtml.AppendHtml(_hyperlinkRenderer.Render(hyperlink)); break;
                            }
                        }
                        break;
                    }
            }
        }

        return th;
    }
}
