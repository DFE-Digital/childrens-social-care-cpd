using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class HeadingRendererBase
{
    public HeadingRendererBase(IRenderer<Text> textRenderer)
    {
        TextRenderer = textRenderer;
    }

    protected IRenderer<Text> TextRenderer { get; }

    protected IHtmlContent HeadingToHtml(string tag, IHeading heading)
    {
        var h = new TagBuilder(tag);

        foreach (var content in heading.Content)
        {
            var text = content as Text;
            if (text != null)
            {
                h.InnerHtml.AppendHtml(TextRenderer.Render(text));
            }
        }

        return h;
    }
}

internal class Heading1Renderer : HeadingRendererBase, IRenderer<Heading1>
{
    public Heading1Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading1 item)
    {
        return HeadingToHtml("h1", item);
    }
}

internal class Heading2Renderer : HeadingRendererBase, IRenderer<Heading2>
{
    public Heading2Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading2 item)
    {
        return HeadingToHtml("h2", item);
    }
}

internal class Heading3Renderer : HeadingRendererBase, IRenderer<Heading3>
{
    public Heading3Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading3 item)
    {
        return HeadingToHtml("h3", item);
    }
}

internal class Heading4Renderer : HeadingRendererBase, IRenderer<Heading4>
{
    public Heading4Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading4 item)
    {
        return HeadingToHtml("h4", item);
    }
}

internal class Heading5Renderer : HeadingRendererBase, IRenderer<Heading5>
{
    public Heading5Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading5 item)
    {
        return HeadingToHtml("h5", item);
    }
}

internal class Heading6Renderer : HeadingRendererBase, IRenderer<Heading6>
{
    public Heading6Renderer(IRenderer<Text> textRenderer) : base(textRenderer)
    { }

    public IHtmlContent Render(Heading6 item)
    {
        return HeadingToHtml("h6", item);
    }
}
