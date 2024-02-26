using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TableHeaderRenderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer) : IRenderer<TableHeader>
{
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
                                case Text text: th.InnerHtml.AppendHtml(textRenderer.Render(text)); break;
                                case Hyperlink hyperlink: th.InnerHtml.AppendHtml(hyperlinkRenderer.Render(hyperlink)); break;
                            }
                        }
                        break;
                    }
            }
        }

        return th;
    }
}
