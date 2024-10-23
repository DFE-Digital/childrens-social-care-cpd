using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class ParagraphRenderer(IRenderer<Text> textRenderer, IRenderer<RoleList> roleListRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer, IRenderer<AreaOfPracticeList> areaOfPracticeListRenderer) : IRendererWithOptions<Paragraph>
{
    public IHtmlContent Render(Paragraph item, RendererOptions options = null)
    {
        TagBuilder p;
        if (options?.RenderInline ?? false) {
            p = new TagBuilder("span");
        }
        else
        {
            p = new TagBuilder("p");
            p.AddCssClass("govuk-body-m");
        }

        foreach (var content in item.Content)
        {
            switch (content)
            {
                case Text text: p.InnerHtml.AppendHtml(textRenderer.Render(text)); break;
                case EntryStructure entryStructure:
                    {
                        switch (entryStructure.Data.Target)
                        {
                            case ContentLink contentLink: p.InnerHtml.AppendHtml(contentLinkRenderer.Render(contentLink)); break;
                            case RoleList roleList: p.InnerHtml.AppendHtml(roleListRenderer.Render(roleList)); break;
                            case AreaOfPracticeList areaOfPracticeList: p.InnerHtml.AppendHtml(areaOfPracticeListRenderer.Render(areaOfPracticeList)); break;
                        }
                        break;
                    }
                case Hyperlink hyperlink: p.InnerHtml.AppendHtml(hyperlinkRenderer.Render(hyperlink)); break;
            }
        }

        return p;
    }

    public IHtmlContent Render(Paragraph item)
    {
        return Render(item, null);
    }
}
