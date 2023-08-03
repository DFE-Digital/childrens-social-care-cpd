using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement("govuk-back-link")]
[OutputElementHint(TagName)]
public class GovUkBackLinkTagHelper : TagHelper
{
    internal const string TagName = "a";

    [HtmlAttributeName("inverse")]
    public bool Inverse { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        IHtmlContent content = output.TagMode == TagMode.StartTagAndEndTag
            ? await output.GetChildContentAsync()
            : new HtmlString("Back");
        
        output.AddClass(Inverse ? "govuk-back-link-inverse" : "govuk-back-link", HtmlEncoder.Default);

        output.TagName = TagName;
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetHtmlContent(content);
    }
}