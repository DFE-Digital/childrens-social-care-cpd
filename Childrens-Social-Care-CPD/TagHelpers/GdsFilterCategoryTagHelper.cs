using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("div")]
[RestrictChildren(GdsFilterCheckboxTagHelper.TagName)]
public class GdsFilterCategoryTagHelper : TagHelper
{
    internal const string TagName = "gds-filter-category";

    [HtmlAttributeName("title")]
    public string Title { get; set; }

    [HtmlAttributeName("index")]
    public int Index { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-accordion__section", HtmlEncoder.Default);

        IHtmlContent content = await output.GetChildContentAsync();

        output.Content.AppendHtml(RenderCategoryHeader(Index, Title));
        output.Content.AppendHtml(RenderCategoryBody(Index, content));
    }

    private static IHtmlContent RenderCategoryHeader(int index, string categoryTitle)
    {
        var headerSpan = new TagBuilder("span");
        headerSpan.AddCssClass("govuk-accordion__section-button");
        headerSpan.Attributes.Add("id", $"accordion-default-heading-{index}");
        headerSpan.InnerHtml.AppendHtml(categoryTitle);

        var h2 = new TagBuilder("h2");
        h2.AddCssClass("govuk-accordion__section-heading");
        h2.InnerHtml.AppendHtml(headerSpan);

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-accordion__section-header");
        div.InnerHtml.AppendHtml(h2);
        return div;
    }

    private static IHtmlContent RenderCategoryBody(int index, IHtmlContent content)
    {
        var fieldsetDiv = new TagBuilder("div");
        fieldsetDiv.AddCssClass("govuk-checkboxes");
        fieldsetDiv.Attributes.Add("data-module", "govuk-checkboxes");

        fieldsetDiv.InnerHtml.AppendHtml(content);

        var fieldset = new TagBuilder("fieldset");
        fieldset.AddCssClass("govuk-fieldset");
        fieldset.InnerHtml.AppendHtml(fieldsetDiv);

        var contentDiv = new TagBuilder("div");
        contentDiv.AddCssClass("govuk-form-group");
        contentDiv.InnerHtml.AppendHtml(fieldset);

        var div = new TagBuilder("div");
        div.AddCssClass("govuk-accordion__section-content");
        div.Attributes.Add("id", $"accordion-default-content-{index}");
        div.Attributes.Add("aria-labelledby", $"accordion-default-heading-{index}");
        div.InnerHtml.AppendHtml(contentDiv);
        return div;
    }
}