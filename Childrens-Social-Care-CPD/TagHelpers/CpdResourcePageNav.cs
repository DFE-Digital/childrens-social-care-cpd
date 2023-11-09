using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("nav")]
public class CpdResourcePageNav : TagHelper
{
    internal const string TagName = "cpd-resource-page-nav";

    [HtmlAttributeName("navigation")]
    public List<ContentLink> Navigation { get; set; }

    [HtmlAttributeName("current")]
    public string Current { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(Current);

        if (Navigation == null || Navigation.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "nav";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-pagination", HtmlEncoder.Default);
        output.AddClass("govuk-pagination--block", HtmlEncoder.Default);
        output.Attributes.Add("role", "navigation");
        output.Attributes.Add("aria-label", "pagination");

        var index = Navigation.FindIndex(x => x.Uri.TrimStart('/') == Current);

        if (index > 0)
        {
            output.Content.AppendHtml(RenderPrevious(Navigation[index - 1]));
        }

        if (index < Navigation.Count-1)
        {
            output.Content.AppendHtml(RenderNext(Navigation[index + 1]));
        }
    }

    private static IHtmlContent RenderNext(ContentLink contentLink)
    {
        var span1 = new TagBuilder("span");
        span1.AddCssClass("govuk-pagination__link-title");
        span1.InnerHtml.AppendHtml("Next");

        var span2 = new TagBuilder("span");
        span2.AddCssClass("govuk-visually-hidden");
        span2.InnerHtml.AppendHtml(":");

        var span3 = new TagBuilder("span");
        span3.AddCssClass("govuk-pagination__link-label");
        span3.InnerHtml.AppendHtml(contentLink.Name);

        var uri = contentLink.Uri.TrimStart('/');
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link govuk-pagination__link");
        anchor.Attributes.Add("href", $"/{uri}");
        anchor.Attributes.Add("rel", "next");
        anchor.InnerHtml.AppendHtml(ArrowRight());
        anchor.InnerHtml.AppendLine(); // This is here to fix a CSS rendering bug in Chrome
        anchor.InnerHtml.AppendHtml(span1);
        anchor.InnerHtml.AppendHtml(span2);
        anchor.InnerHtml.AppendHtml(span3);

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-pagination__next");
        div.InnerHtml.AppendHtml(anchor);

        return div;
    }

    private static IHtmlContent RenderPrevious(ContentLink contentLink)
    {
        var span1 = new TagBuilder("span");
        span1.AddCssClass("govuk-pagination__link-title");
        span1.InnerHtml.AppendHtml("Previous");

        var span2 = new TagBuilder("span");
        span2.AddCssClass("govuk-visually-hidden");
        span2.InnerHtml.AppendHtml(":");

        var span3 = new TagBuilder("span");
        span3.AddCssClass("govuk-pagination__link-label");
        span3.InnerHtml.AppendHtml(contentLink.Name);

        var uri = contentLink.Uri.TrimStart('/');
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link govuk-pagination__link");
        anchor.Attributes.Add("href", $"/{uri}");
        anchor.Attributes.Add("rel", "prev");
        anchor.InnerHtml.AppendHtml(ArrowLeft());
        anchor.InnerHtml.AppendLine(); // This is here to fix a CSS rendering bug in Chrome
        anchor.InnerHtml.AppendHtml(span1);
        anchor.InnerHtml.AppendHtml(span2);
        anchor.InnerHtml.AppendHtml(span3);

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-pagination__prev");
        div.InnerHtml.AppendHtml(anchor);

        return div;
    }

    private static IHtmlContent ArrowRight()
    {
        var path = new TagBuilder("path");
        path.Attributes.Add("d", "m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z");

        var svg = new TagBuilder("svg");
        svg.AddCssClass("govuk-pagination__icon govuk-pagination__icon--next");
        svg.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
        svg.Attributes.Add("height", "13");
        svg.Attributes.Add("width", "15");
        svg.Attributes.Add("aria-hidden", "true");
        svg.Attributes.Add("focusable", "false");
        svg.Attributes.Add("viewBox", "0 0 15 13");

        svg.InnerHtml.AppendHtml(path);
        svg.InnerHtml.AppendLine();
        return svg;
    }

    private static IHtmlContent ArrowLeft()
    {
        var path = new TagBuilder("path");
        path.Attributes.Add("d", "m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z");

        var svg = new TagBuilder("svg");
        svg.AddCssClass("govuk-pagination__icon govuk-pagination__icon--prev");
        svg.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
        svg.Attributes.Add("height", "13");
        svg.Attributes.Add("width", "15");
        svg.Attributes.Add("aria-hidden", "true");
        svg.Attributes.Add("focusable", "false");
        svg.Attributes.Add("viewBox", "0 0 15 13");

        svg.InnerHtml.AppendHtml(path);
        return svg;
    }

}