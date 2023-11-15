using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Childrens_Social_Care_CPD.TagHelpers;

[HtmlTargetElement(TagName)]
[OutputElementHint("nav")]
public class CpdResourceNav : TagHelper
{
    internal const string TagName = "cpd-resource-nav";

    [HtmlAttributeName("navigation")]
    public IList<ContentLink> Navigation { get; set; }

    [HtmlAttributeName("selected")]
    public string Selected { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Navigation == null || Navigation.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "nav";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("gem-c-contents-list", HtmlEncoder.Default);
        output.Attributes.Add("role", "navigation");
        output.Attributes.Add("aria-label", "Resource pages");

        var h2 = new TagBuilder("h2");
        h2.AddCssClass("gem-c-contents-list__title");
        h2.InnerHtml.Append("Content");

        output.Content.AppendHtml(h2);
        output.Content.AppendHtml(RenderNavigation(Navigation, Selected));
    }

    private static IHtmlContent RenderNavigation(IList<ContentLink> navigation, string selected)
    {
        var ol = new TagBuilder("ol");
        ol.AddCssClass("gem-c-contents-list__list");

        foreach (var contentLink in navigation)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("gem-c-contents-list__list-item gem-c-contents-list__list-item--dashed");

            var uri = contentLink.Uri.TrimStart('/');
            if (uri == selected)
            {
                li.AddCssClass("gem-c-contents-list__list-item--active");
                li.Attributes.Add("aria-current", "true");
                li.InnerHtml.Append(contentLink.Name);
            }
            else
            {
                var a = new TagBuilder("a");
                a.AddCssClass("gem-c-contents-list__link govuk-link");
                a.Attributes.Add("href", $"/{uri}");
                a.InnerHtml.Append(contentLink.Name);
                li.InnerHtml.AppendHtml(a);
            }

            ol.InnerHtml.AppendHtml(li);
        }

        return ol;
    }
}