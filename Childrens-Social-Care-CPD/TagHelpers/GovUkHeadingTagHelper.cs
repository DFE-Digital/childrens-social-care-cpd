using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement("govuk-heading")]
public class GovUkHeadingTagHelper : TagHelper
{
    [HtmlAttributeName("level")]
    public int Level { get; set; }

    [HtmlAttributeName("display-size")]
    public int DisplaySize { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Level = Math.Clamp(Level, 1, 4);

        output.TagName = $"h{Level}";

        switch (DisplaySize)
        {
            case 1: output.AddClass("govuk-heading-xl", HtmlEncoder.Default); break;
            case 2: output.AddClass("govuk-heading-l", HtmlEncoder.Default); break;
            case 3: output.AddClass("govuk-heading-m", HtmlEncoder.Default); break;
            case 4: output.AddClass("govuk-heading-s", HtmlEncoder.Default); break;
        }
    }
}
