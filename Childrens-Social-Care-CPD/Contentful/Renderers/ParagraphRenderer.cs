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
    private readonly IRenderer<ContentLink> _contentLinkRenderer;
    private readonly IRenderer<AreaOfPracticeList> _areaOfPracticeList;


    public ParagraphRenderer(IRenderer<Text> textRenderer, IRenderer<RoleList> roleListRenderer, IRenderer<Hyperlink> hyperlinkRenderer, IRenderer<ContentLink> contentLinkRenderer, IRenderer<AreaOfPracticeList> areaOfPracticeList)
    {
        _textRenderer = textRenderer;
        _roleListRenderer = roleListRenderer;
        _hyperlinkRenderer = hyperlinkRenderer;
        _contentLinkRenderer = contentLinkRenderer;
        _areaOfPracticeList = areaOfPracticeList;
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
                            case ContentLink contentLink: p.InnerHtml.AppendHtml(_contentLinkRenderer.Render(contentLink)); break;
                            case RoleList roleList: p.InnerHtml.AppendHtml(_roleListRenderer.Render(roleList)); break;
                            case AreaOfPracticeList areaOfPracticeList: p.InnerHtml.AppendHtml(_areaOfPracticeList.Render(areaOfPracticeList)); break;
                        }
                        break;
                    }
                case Hyperlink hyperlink: p.InnerHtml.AppendHtml(_hyperlinkRenderer.Render(hyperlink)); break;
            }
        }

        return p;
    }
}
