using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful;

public static class RoleListExtensions
{
    private static IHtmlContent NoRoles()
    {
        var span = new TagBuilder("space");
        span.InnerHtml.Append("No Roles Available");
        return span;
    }

    private static IHtmlContent RoleTitle(string id, DetailedRole detailedRole)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-heading-s govuk-!-margin-bottom-1");
        var heading3 = new TagBuilder("h3");

        heading3.InnerHtml.AppendHtml(ContentLink(detailedRole.Title, id));

        div.InnerHtml.AppendHtml(heading3);
        return div;
    }

    public static IHtmlContent ContentLink(string text, string uri)
    {
        var tagBuilder = new TagBuilder("a");

        var href = uri switch
        {
            string s when s.StartsWith("http") => s,
            string s when s.StartsWith('/') => s,
            _ => string.Format("/{0}", uri)
        };

        tagBuilder.Attributes.Add("href", href);
        tagBuilder.AddCssClass("govuk-link");
        tagBuilder.InnerHtml.Append(text);
        return tagBuilder;
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

    public static IHtmlContent ToHtml(this RoleList roleList)
    {
        if (roleList.Roles.Count == 0)
        {
            return NoRoles();
        }

        var htmlContentBuilder = new HtmlContentBuilder();

        foreach (var contentItem in roleList.Roles)
        {
            var detailedRole = contentItem.Items.FirstOrDefault(x => typeof(DetailedRole) == x.GetType()) as DetailedRole;
            if (detailedRole == null) continue;

            htmlContentBuilder.AppendHtml(RoleTitle(contentItem.Id, detailedRole));
            htmlContentBuilder.AppendHtml(RoleSummary(detailedRole));
        }

        return htmlContentBuilder;
    }
}
