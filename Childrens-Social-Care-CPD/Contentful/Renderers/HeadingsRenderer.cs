using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class HeadingRendererBase
{
    private readonly IRenderer<Text> _textRenderer;
    private readonly IRenderer<Hyperlink> _hyperlinkRenderer;
    private readonly IRenderer<ContentLink> _contentLinkRenderer;

    public HeadingRendererBase(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer)
    {
        _textRenderer = textRenderer;
        _hyperlinkRenderer = hyperlinkRenderer;
        _contentLinkRenderer = contentLinkRenderer;
    }

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
                                        case ContentLink contentLink: h.InnerHtml.AppendHtml(_contentLinkRenderer.Render(contentLink)); break;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Text text: h.InnerHtml.AppendHtml(_textRenderer.Render(text)); break;
                case Hyperlink hyperlink: h.InnerHtml.AppendHtml(_hyperlinkRenderer.Render(hyperlink)); break;
            }
        }

        return h;
    }
}

internal class Heading1Renderer : HeadingRendererBase, IRenderer<Heading1>
{
    public Heading1Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading1 item)
    {
        return HeadingToHtml("h1", item);
    }
}

internal class Heading2Renderer : HeadingRendererBase, IRenderer<Heading2>
{
    public Heading2Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading2 item)
    {
        return HeadingToHtml("h2", item);
    }
}

internal class Heading3Renderer : HeadingRendererBase, IRenderer<Heading3>
{
    public Heading3Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading3 item)
    {
        return HeadingToHtml("h3", item);
    }
}

internal class Heading4Renderer : HeadingRendererBase, IRenderer<Heading4>
{
    public Heading4Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading4 item)
    {
        return HeadingToHtml("h4", item);
    }
}

internal class Heading5Renderer : HeadingRendererBase, IRenderer<Heading5>
{
    public Heading5Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading5 item)
    {
        return HeadingToHtml("h5", item);
    }
}

internal class Heading6Renderer : HeadingRendererBase, IRenderer<Heading6>
{
    public Heading6Renderer(IRenderer<Text> textRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer) : base(textRenderer, hyperlinkRenderer, contentLinkRenderer)
    { }

    public IHtmlContent Render(Heading6 item)
    {
        return HeadingToHtml("h6", item);
    }
}
