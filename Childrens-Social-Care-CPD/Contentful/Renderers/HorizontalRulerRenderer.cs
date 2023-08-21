using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class HorizontalRulerRenderer : IRenderer<HorizontalRuler>
{
    public IHtmlContent Render(HorizontalRuler item)
    {
        var hr = new TagBuilder("hr")
        {
            TagRenderMode = TagRenderMode.SelfClosing
        };
        hr.AddCssClass("govuk-section-break govuk-section-break--m govuk-section-break--visible");
        return hr;
    }
}
