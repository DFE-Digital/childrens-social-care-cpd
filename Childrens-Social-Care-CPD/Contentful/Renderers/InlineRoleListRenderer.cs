using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class InlineRoleListRenderer : IRenderer<RoleList>
{
    private readonly IRenderer<ContentLink> _contentLinkRenderer;

    public InlineRoleListRenderer(IRenderer<ContentLink> contentLinkRenderer)
    {
        _contentLinkRenderer = contentLinkRenderer;
    }

    public IHtmlContent Render(RoleList item)
    {
        if (item.Roles.Count == 0)
        {
            return NoRoles();
        }

        var htmlContentBuilder = new HtmlContentBuilder();

        foreach (var contentItem in item.Roles)
        {
            var detailedRole = contentItem.Items.Find(x => typeof(DetailedRole) == x.GetType()) as DetailedRole;
            if (detailedRole == null) continue;

            htmlContentBuilder.AppendHtml(RoleTitle(contentItem.Id, detailedRole));
            htmlContentBuilder.AppendHtml(RoleSummary(detailedRole));
        }

        return htmlContentBuilder;
    }

    private static IHtmlContent NoRoles()
    {
        var span = new TagBuilder("span");
        span.InnerHtml.Append("No Roles Available");
        return span;
    }

    private IHtmlContent RoleTitle(string id, DetailedRole detailedRole)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-heading-s govuk-!-margin-bottom-1");
        var heading3 = new TagBuilder("h3");

        var contentLink = new ContentLink()
        {
            Name = detailedRole.Title,
            Uri = id
        };

        heading3.InnerHtml.AppendHtml(_contentLinkRenderer.Render(contentLink));

        div.InnerHtml.AppendHtml(heading3);
        return div;
    }

    private static IHtmlContent RoleSummary(DetailedRole detailedRole)
    {
        var htmlContentBuilder = new HtmlContentBuilder();
        var p = new TagBuilder("p");
        p.AddCssClass("govuk-body-s");
        p.InnerHtml.Append(detailedRole.Summary);
        p.InnerHtml.Append(".");

        var rowDiv = new TagBuilder("div");
        rowDiv.AddCssClass("govuk-grid-row govuk-!-padding-bottom-3");

        var columnDiv = new TagBuilder("div");
        columnDiv.AddCssClass("govuk-grid-column-one-half");

        var ul = new TagBuilder("ul");
        ul.AddCssClass("govuk-list");

        var li = new TagBuilder("li");

        var span = new TagBuilder("span");
        span.AddCssClass("govuk-!-font-size-16 govuk-!-font-weight-bold");
        span.InnerHtml.Append("Salary range: ");
        span.InnerHtml.Append(detailedRole.SalaryRange);

        li.InnerHtml.AppendHtml(span);
        ul.InnerHtml.AppendHtml(li);
        columnDiv.InnerHtml.AppendHtml(ul);
        rowDiv.InnerHtml.AppendHtml(columnDiv);

        htmlContentBuilder.AppendHtml(p);
        htmlContentBuilder.AppendHtml(rowDiv);

        return htmlContentBuilder;
    }
}
