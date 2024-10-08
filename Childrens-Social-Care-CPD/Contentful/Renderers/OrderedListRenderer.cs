using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Text = Contentful.Core.Models.Text;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public class OrderedListRenderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer) : IRenderer<List>
{
    public IHtmlAsyncContent Render(List item)
    {
        if (item.Content.Count == 0)
        {
            return null;
        }

        var ol = new TagBuilder("ol");
        ol.AddCssClass("govuk-list govuk-list--number");

        foreach (var listItem in item.Content.OfType<ListItem>())
        {
            if (listItem.Content[0] is not Paragraph paragraph) continue;

            var li = new TagBuilder("li");
            foreach (var content in paragraph.Content)
            {
                switch (content)
                {
                    case Text text:
                        {
                            li.InnerHtml.AppendHtml(textRenderer.Render(text));
                            break;
                        }
                    case Hyperlink hyperlink:
                        {
                            li.InnerHtml.AppendHtml(hyperlinkRenderer.Render(hyperlink));
                            break;
                        }
                }
            }
            ol.InnerHtml.AppendHtml(li);
        }
        return (IHtmlAsyncContent)ol;
    }

    IHtmlContent IRenderer<List>.Render(List item)
    {
        throw new NotImplementedException();
    }
}