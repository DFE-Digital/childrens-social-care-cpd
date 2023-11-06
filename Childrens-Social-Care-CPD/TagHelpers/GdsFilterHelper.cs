using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("div")]
public class GdsFilterTagHelper : TagHelper
{
    internal const string TagName = "gds-filter";

    [HtmlAttributeName("clear-filters-uri")]
    public string ClearFiltersUri { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.AddClass("govuk-summary-card", HtmlEncoder.Default);
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        IHtmlContent content = await output.GetChildContentAsync();

        output.Content.AppendHtml(RenderHeader(ClearFiltersUri));
        output.Content.AppendHtml(RenderBody(content));
    }

    private static IHtmlContent RenderBody(IHtmlContent content)
    {
        var button = new TagBuilder("button");
        button.AddCssClass("govuk-button");
        button.Attributes.Add("data-module", "govuk-button");
        button.InnerHtml.Append("Apply filter");

        var innerDiv = new TagBuilder("div");
        innerDiv.AddCssClass("govuk-accordion");
        innerDiv.Attributes.Add("id", "accordion-default");
        innerDiv.Attributes.Add("data-module", "govuk-accordion");
        innerDiv.InnerHtml.AppendHtml(content);
        
        var form = new TagBuilder("form");
        form.Attributes.Add("id", "filter-form");
        form.Attributes.Add("method", "get");
        form.InnerHtml.AppendHtml(innerDiv);
        form.InnerHtml.AppendHtml(button);

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-summary-card__content");
        div.InnerHtml.AppendHtml(form);
        return div;
    }

    private static IHtmlContent RenderHeader(string path)
    {
        var h2 = new TagBuilder("h2");
        h2.AddCssClass("govuk-heading-m");
        h2.InnerHtml.Append("Filters");

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-summary-card__title-wrapper");
        div.InnerHtml.AppendHtml(h2);
        div.InnerHtml.AppendHtml(RenderActions(path));
        return div;
    }

    private static IHtmlContent RenderActions(string path)
    {
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link");

        anchor.InnerHtml.Append("Clear filters");
        anchor.Attributes.Add("href", path);

        var li = new TagBuilder("li");
        li.AddCssClass("govuk-summary-card__action");
        li.InnerHtml.AppendHtml(anchor);

        var ul = new TagBuilder("ul");
        ul.AddCssClass("govuk-summary-card__actions");
        ul.InnerHtml.AppendHtml(li);

        return ul;
    }
}