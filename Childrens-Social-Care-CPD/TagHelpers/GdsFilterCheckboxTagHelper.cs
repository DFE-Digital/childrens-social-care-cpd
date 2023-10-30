using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("div")]
public class GdsFilterCheckboxTagHelper : TagHelper
{
    internal const string TagName = "gds-filter-checkbox";

    [HtmlAttributeName("id")]
    public string Id { get; set; }

    [HtmlAttributeName("name")]
    public string Name { get; set; }

    [HtmlAttributeName("checked")]
    public bool Checked { get; set; }

    [HtmlAttributeName("value")]
    public string Value { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-checkboxes__item", HtmlEncoder.Default);

        IHtmlContent content = await output.GetChildContentAsync();

        var input = new TagBuilder("input");
        input.AddCssClass("govuk-checkboxes__input");

        input.Attributes.Add("type", "checkbox");
        input.Attributes.Add("id", Id);
        input.Attributes.Add("name", Name);
        input.Attributes.Add("value", Value);
        if (Checked)
        {
            input.Attributes.Add("checked", "");
        }

        var label = new TagBuilder("label");
        label.AddCssClass("govuk-label govuk-checkboxes__label");
        label.Attributes.Add("for", Id);
        label.InnerHtml.AppendHtml(content);

        output.Content.AppendHtml(input);
        output.Content.AppendHtml(label);
    }
}