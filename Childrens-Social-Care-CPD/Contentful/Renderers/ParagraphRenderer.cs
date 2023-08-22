using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class ParagraphRenderer : IRenderer<Paragraph>
{
    private readonly IRenderer<Text> _textRenderer;
    private readonly IRenderer<RoleList> _roleListRenderer;
    private readonly IRenderer<Hyperlink> _hyperlinkRenderer;

    public ParagraphRenderer(IRenderer<Text> textRenderer, IRenderer<RoleList> roleListRenderer, IRenderer<Hyperlink> hyperlinkRenderer)
    {
        _textRenderer = textRenderer;
        _roleListRenderer = roleListRenderer;
        _hyperlinkRenderer = hyperlinkRenderer;
    }

    public IHtmlContent Render(Paragraph item)
    {
        var p = new TagBuilder("p");
        p.AddCssClass("govuk-body-m");

        foreach (var content in item.Content)
        {
            switch (content)
            {
                case Text text: p.InnerHtml.AppendHtml(_textRenderer.Render(text)); break;
                case EntryStructure entryStructure:
                    {
                        switch (entryStructure.Data.Target)
                        {
                            case RoleList roleList: p.InnerHtml.AppendHtml(_roleListRenderer.Render(roleList)); break;
                        }
                        break;
                    }
                case Hyperlink hyperlink: p.InnerHtml.AppendHtml(_hyperlinkRenderer.Render(hyperlink)); break;
            }
        }

        return p;
    }
}
