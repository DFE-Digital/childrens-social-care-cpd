using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement("govuk-back-link")]
public class GovUkBackLinkTagHelper : TagHelper
{
    [HtmlAttributeName("href")]
    public string Href { get; set; }

    [HtmlAttributeName("onclick")]
    public string OnClick { get; set; }

    [HtmlAttributeName("inverse")]
    public bool Inverse { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = $"a";
        output.AddClass(Inverse ? "govuk-back-link-inverse" : "govuk-back-link", HtmlEncoder.Default);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("href", Href);

        if (!string.IsNullOrEmpty(OnClick))
        {
            output.Attributes.SetAttribute("onClick", OnClick);
        }

        var content = output.GetChildContentAsync().Result.GetContent();
        if (string.IsNullOrEmpty(content))
        {
            output.Content.SetContent("Back");
        }

        return base.ProcessAsync(context, output);
    }
}
