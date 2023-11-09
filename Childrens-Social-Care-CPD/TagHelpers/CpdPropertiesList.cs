using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("ul")]
public class CpdPropertiesList : TagHelper
{
    internal const string TagName = "cpd-properties-list";

    [HtmlAttributeName("items")]
    public IReadOnlyDictionary<string, string> Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Items == null || Items.Count == 0 )
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "ul";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("course-details", HtmlEncoder.Default);
        output.AddClass("govuk-list", HtmlEncoder.Default);

        foreach (var item in Items)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("govuk-!-font-size-16");
            span.InnerHtml.Append($"{item.Key}:&nbsp{item.Value}");

            var li = new TagBuilder("li");
            li.InnerHtml.AppendHtml(span);

            output.Content.AppendHtml(li);
        }
    }
}