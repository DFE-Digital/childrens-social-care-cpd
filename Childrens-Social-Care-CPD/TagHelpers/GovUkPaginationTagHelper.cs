using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement("govuk-pagination")]
public class GovUkPaginationTagHelper : TagHelper
{
    [HtmlAttributeName("url-format-string")]
    public string UrlFormatString { get; set; }

    [HtmlAttributeName("page-count")]
    public int PageCount { get; set; }

    [HtmlAttributeName("current-page")]
    public int CurrentPage { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PageCount == 0)
        {
            output.TagName = null;
            return;
        }

        output.TagName = "nav";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-pagination", HtmlEncoder.Default);

        if (CurrentPage > 1)
        {
            output.Content.AppendHtml(GeneratePreviousButton(string.Format(UrlFormatString, CurrentPage - 1)));
        }

        var pageLinks = GeneratesPagesPages(PageCount, CurrentPage, UrlFormatString);
        output.Content.AppendHtml(pageLinks);

        if (CurrentPage < PageCount)
        {
            output.Content.AppendHtml(GenerateNextButton(string.Format(UrlFormatString, CurrentPage + 1)));
        }
    }

    private static IHtmlContent GeneratesPagesPages(int pageCount, int currentPage, string formatString)
    {
        var ul = new TagBuilder("ul");
        ul.AddCssClass("govuk-pagination__list");

        for (var index = 1; index <= pageCount; index++)
        {
            switch (index)
            {
                case int i when i == 1 || i == pageCount || (i >= currentPage - 1 && i <= currentPage + 1):
                    {
                        ul.InnerHtml.AppendHtml(GeneratePageNumber(index, string.Format(formatString, index), i == currentPage));
                        break;
                    }
                case int i when i == currentPage - 2 || i == currentPage + 2:
                    {
                        ul.InnerHtml.AppendHtml(GenerateEllipses());
                        break;
                    }
                default:
                        continue;
            }
        }

        return ul;
    }

    private static IHtmlContent GenerateEllipses()
    {
        var li = new TagBuilder("li");
        li.AddCssClass("govuk-pagination__item govuk-pagination__item--ellipses");
        li.InnerHtml.AppendHtml(new HtmlString("&ctdot;"));
        return li;
    }

    private static IHtmlContent GeneratePageNumber(int pageNumber, string href, bool isCurrent)
    {
        var listItem = new TagBuilder("li");
        listItem.AddCssClass("govuk-pagination__item");
        if (isCurrent)
        {
            listItem.AddCssClass("govuk-pagination__item--current");
        }

        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link govuk-pagination__link");
        anchor.Attributes.Add("href", href);
        anchor.Attributes.Add("aria-label", $"Page {pageNumber}");
        if (isCurrent)
        {
            anchor.Attributes.Add("aria-current", $"page");
        }
        anchor.InnerHtml.Append($"{pageNumber}");

        listItem.InnerHtml.AppendHtml(anchor);
        return listItem;
    }

    public IHtmlContent GeneratePreviousButton(string href)
    {
        var path = new TagBuilder("path");
        path.Attributes.Add("d", "m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z");

        var svg = new TagBuilder("svg");
        svg.AddCssClass("govuk-pagination__icon govuk-pagination__icon--prev");
        svg.Attributes.Add("xlmns", "http://www.w3.org/2000/svg");
        svg.Attributes.Add("height", "13");
        svg.Attributes.Add("width", "15");
        svg.Attributes.Add("aria-hidden", "true");
        svg.Attributes.Add("focusable", "false");
        svg.Attributes.Add("viewBox", "0 0 15 13");
        svg.InnerHtml.AppendHtml(path);
        
        var span = new TagBuilder("span");
        span.AddCssClass("govuk-pagination__link-title");
        span.InnerHtml.Append("Previous");

        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link govuk-pagination__link");
        anchor.Attributes.Add("href", href);
        anchor.Attributes.Add("rel", "prev");
        anchor.InnerHtml.AppendHtml(svg);
        anchor.InnerHtml.AppendHtml(span);
        
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-pagination__prev");
        div.InnerHtml.AppendHtml(anchor);
        return div;
    }

    public IHtmlContent GenerateNextButton(string href)
    {
        var path = new TagBuilder("path");
        path.Attributes.Add("d", "m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z");
        
        var svg = new TagBuilder("svg");
        svg.AddCssClass("govuk-pagination__icon govuk-pagination__icon--next");
        svg.Attributes.Add("xlmns", "http://www.w3.org/2000/svg");
        svg.Attributes.Add("height", "13");
        svg.Attributes.Add("width", "15");
        svg.Attributes.Add("aria-hidden", "true");
        svg.Attributes.Add("focusable", "false");
        svg.Attributes.Add("viewBox", "0 0 15 13");
        svg.InnerHtml.AppendHtml(path);
        
        var span = new TagBuilder("span");
        span.AddCssClass("govuk-pagination__link-title");
        span.InnerHtml.Append("Next");
        
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link govuk-pagination__link");
        anchor.Attributes.Add("href", href);
        anchor.Attributes.Add("rel", "next");
        anchor.InnerHtml.AppendHtml(span);
        anchor.InnerHtml.AppendHtml(svg);

        
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-pagination__next");
        div.InnerHtml.AppendHtml(anchor);
        return div;
    }
}
