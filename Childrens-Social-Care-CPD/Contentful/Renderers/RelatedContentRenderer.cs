using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public class RelatedContentRenderer : IRenderer<RelatedContent>
{
    private readonly IRenderer<ContentLink> _contentLinkRenderer;

    public RelatedContentRenderer(IRenderer<ContentLink> contentLinkRenderer)
    {
        _contentLinkRenderer = contentLinkRenderer;
    }

    public IHtmlContent Render(RelatedContent item)
    {
        if (item == null) return null;

        var rowDiv = new TagBuilder("div");
        rowDiv.AddCssClass("govuk-grid-row");
        
        var columnDiv = new TagBuilder("div");
        columnDiv.AddCssClass("govuk-grid-column-two-thirds");

        var marginDiv = new TagBuilder("div");
        marginDiv.AddCssClass("govuk-!-margin-top-28");

        var aside = new TagBuilder("aside");
        aside.AddCssClass("app-related-items");
        aside.Attributes.Add("role", "complementary");

        var h2 = RenderHeading();
        var nav = RenderNav(item);

        aside.InnerHtml.AppendHtml(h2);
        aside.InnerHtml.AppendHtml(nav);
        marginDiv.InnerHtml.AppendHtml(aside);
        columnDiv.InnerHtml.AppendHtml(marginDiv);
        rowDiv.InnerHtml.AppendHtml(columnDiv);
        return rowDiv;
    }

    private static IHtmlContent RenderHeading()
    {
        var h2 = new TagBuilder("h2");
        h2.AddCssClass("govuk-heading-s");
        h2.Attributes.Add("id", "related-nav__section");
        h2.InnerHtml.Append("Related content");
        return h2;
    }

    private IHtmlContent RenderNav(RelatedContent relatedContent)
    {
        var nav = new TagBuilder("nav");
        nav.Attributes.Add("role", "navigation");
        nav.Attributes.Add("aria-labelledby", "related-nav__section");

        var ul = new TagBuilder("ul");
        ul.AddCssClass("govuk-list govuk-!-font-size-16");

        foreach(var contentLink in relatedContent)
        {
            var li = new TagBuilder("li");
            li.InnerHtml.AppendHtml(_contentLinkRenderer.Render(contentLink));
            ul.InnerHtml.AppendHtml(li);
        }

        nav.InnerHtml.AppendHtml(ul);
        return nav;
    }
}
