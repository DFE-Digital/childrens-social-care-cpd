using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class HeadingRendererBase(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer)
{
    protected TagBuilder HeadingToHtml(string tag, IHeading heading)
    {
        var h = new TagBuilder(tag);
        foreach (var content in heading.Content)
        {
            switch (content)
            {
                case EntryStructure entryStructure:
                    {
                        switch (entryStructure.Data)
                        {
                            case EntryStructureData entryStructureData:
                                {
                                    switch (entryStructureData.Target)
                                    {
                                        case ContentLink contentLink: h.InnerHtml.AppendHtml(contentLinkRenderer.Render(contentLink)); break;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Text text: h.InnerHtml.AppendHtml(textRenderer.Render(text)); break;
                case Hyperlink hyperlink: h.InnerHtml.AppendHtml(hyperlinkRenderer.Render(hyperlink)); break;
            }
        }

        return h;
    }
}

internal class Heading1Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading1>
{
    public IHtmlContent Render(Heading1 item)
    {
        return HeadingToHtml("h1", item);
    }
}

internal class Heading2Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading2>
{
    public IHtmlContent Render(Heading2 item)
    {
        return HeadingToHtml("h2", item);
    }
}

internal class Heading3Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading3>
{
    public IHtmlContent Render(Heading3 item)
    {
        return HeadingToHtml("h3", item);
    }
}

internal class Heading4Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading4>
{
    public IHtmlContent Render(Heading4 item)
    {
        return HeadingToHtml("h4", item);
    }
}

internal class Heading5Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading5>
{
    public IHtmlContent Render(Heading5 item)
    {
        return HeadingToHtml("h5", item);
    }
}

internal class Heading6Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : HeadingRendererBase(textRenderer, hyperlinkRenderer, contentLinkRenderer), IRenderer<Heading6>
{
    public IHtmlContent Render(Heading6 item)
    {
        return HeadingToHtml("h6", item);
    }
}
