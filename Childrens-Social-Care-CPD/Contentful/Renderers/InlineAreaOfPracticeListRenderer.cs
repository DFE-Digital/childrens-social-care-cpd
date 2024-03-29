﻿using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class InlineAreaOfPracticeListRenderer(IRenderer<ContentLink> contentLinkRenderer) : IRenderer<AreaOfPracticeList>
{
    public IHtmlContent Render(AreaOfPracticeList item)
    {
        if (item.Areas.Count == 0)
        {
            return NoAreasOfpractice();
        }

        var htmlContentBuilder = new HtmlContentBuilder();

        foreach (var contentItem in item.Areas)
        {
            var areaOfpractice = contentItem.Items.OfType<AreaOfPractice>().FirstOrDefault();
            if (areaOfpractice == null) continue;

            htmlContentBuilder.AppendHtml(AreaOfPracticeTitle(contentItem.Id, areaOfpractice));
            htmlContentBuilder.AppendHtml(AreaOfPracticeSummary(areaOfpractice));
        }

        return htmlContentBuilder;
    }

    private static TagBuilder NoAreasOfpractice()
    {
        var span = new TagBuilder("span");
        span.InnerHtml.Append("No Areas Of practice Available");
        return span;
    }

    private TagBuilder AreaOfPracticeTitle(string id, AreaOfPractice areaOfPractice)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-heading-s govuk-!-margin-bottom-1");
        var heading2 = new TagBuilder("h2");

        var contentLink = new ContentLink()
        {
            Name = areaOfPractice.Title,
            Uri = id
        };

        heading2.InnerHtml.AppendHtml(contentLinkRenderer.Render(contentLink));

        div.InnerHtml.AppendHtml(heading2);
        return div;
    }

    private static HtmlContentBuilder AreaOfPracticeSummary(AreaOfPractice areaOfPractice)
    {
        var htmlContentBuilder = new HtmlContentBuilder();
        var p = new TagBuilder("p");
        p.AddCssClass("govuk-body");
        p.InnerHtml.Append(areaOfPractice.AreaOfPracticeListSummary);
        htmlContentBuilder.AppendHtml(p);
        return htmlContentBuilder;
    }
}
