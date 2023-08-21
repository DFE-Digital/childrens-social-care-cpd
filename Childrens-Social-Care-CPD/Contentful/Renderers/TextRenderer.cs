using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TextRenderer : IRenderer<Text>
{
    public IHtmlContent Render(Text item)
    {
        if (string.IsNullOrEmpty(item.Value))
            return null;

        var htmlContentBuilder = new HtmlContentBuilder();

        var parts = item.Value.Split("\n");
        for (var index = 0; index < parts.Length - 1; index++)
        {
            htmlContentBuilder.Append(parts[index]);
            htmlContentBuilder.AppendHtml("<br>");
        }
        htmlContentBuilder.Append(parts.Last());

        IHtmlContent htmlContent = htmlContentBuilder;
        foreach (var mark in item.Marks)
        {
            switch (mark.Type)
            {
                case "bold": htmlContent = EncaseInTag(htmlContent, "strong"); break;
                case "italic": htmlContent = EncaseInTag(htmlContent, "i"); break;
                case "underline": htmlContent = EncaseInTag(htmlContent, "u"); break;
                case "code": htmlContent = EncaseInTag(htmlContent, "code"); break;
                case "superscript": htmlContent = EncaseInTag(htmlContent, "sup"); break;
                case "subscript": htmlContent = EncaseInTag(htmlContent, "sub"); break;
            }
        }

        return htmlContent;
    }

    private IHtmlContent EncaseInTag(IHtmlContent htmlContent, string tag)
    {
        var tagBuilder = new TagBuilder(tag);
        tagBuilder.InnerHtml.SetHtmlContent(htmlContent);
        return tagBuilder;
    }
}
