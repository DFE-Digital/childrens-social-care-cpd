using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Text = Contentful.Core.Models.Text;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public class ListRenderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer) : IRenderer<List>
{
    public IHtmlContent Render(List item)
    {
        if (item.Content.Count == 0)
        {
            return null;
        }

        // if node type = ol then use tag builder for orderedlist
        TagBuilder listTag;
        string cssClass;
        if (item.NodeType == "unordered-list")
        {
            listTag = new TagBuilder("ul");
            cssClass = "govuk-list govuk-list--bullet";
        }
        else
        {
            listTag = new TagBuilder("ol");
            cssClass = "govuk-list govuk-list--number";
        }
        listTag.AddCssClass(cssClass);

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
            listTag.InnerHtml.AppendHtml(li);
        }

        return listTag;
    }
}