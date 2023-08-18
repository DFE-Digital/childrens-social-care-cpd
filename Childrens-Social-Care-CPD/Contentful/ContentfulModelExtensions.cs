using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful;

public static class ContentfulModelExtensions
{
    private static IHtmlContent EncaseInTag(IHtmlContent htmlContent, string tag)
    {
        var tagBuilder = new TagBuilder(tag);
        tagBuilder.InnerHtml.SetHtmlContent(htmlContent);
        return tagBuilder;
    }

    public static IHtmlContent ToHtml(this Text text)
    {
        if (string.IsNullOrEmpty(text.Value)) 
            return null;

        var htmlContentBuilder = new HtmlContentBuilder();

        var parts = text.Value.Split("\n");
        for (var index = 0; index < parts.Length - 1; index++)
        {
            htmlContentBuilder.Append(parts[index]);
            htmlContentBuilder.AppendHtml("<br>");
        }
        htmlContentBuilder.Append(parts.Last());

        IHtmlContent htmlContent = htmlContentBuilder;
        foreach (var mark in text.Marks)
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

    public static IHtmlContent ToHtml(this Hyperlink hyperlink)
    {
        var linkText = (hyperlink.Content.FirstOrDefault() as Text)?.Value;
        var anchor = new TagBuilder("a");
        anchor.AddCssClass("govuk-link");
        anchor.Attributes.Add("href", hyperlink.Data.Uri);
        anchor.InnerHtml.SetContent(linkText);
        return anchor;
    }

    private static IHtmlContent HeadingToHtml(string tag, IHeading heading)
    {
        var h = new TagBuilder(tag);

        foreach (var content in heading.Content)
        {
            var text = content as Text;
            if (text != null)
            {
                h.InnerHtml.AppendHtml(text.ToHtml());
            }
        }

        return h;
    }

    public static IHtmlContent ToHtml(this Heading1 heading)
    {
        return HeadingToHtml("h1", heading);
    }

    public static IHtmlContent ToHtml(this Heading2 heading)
    {
        return HeadingToHtml("h2", heading);
    }

    public static IHtmlContent ToHtml(this Heading3 heading)
    {
        return HeadingToHtml("h3", heading);
    }

    public static IHtmlContent ToHtml(this Heading4 heading)
    {
        return HeadingToHtml("h4", heading);
    }

    public static IHtmlContent ToHtml(this Heading5 heading)
    {
        return HeadingToHtml("h5", heading);
    }

    public static IHtmlContent ToHtml(this Heading6 heading)
    {
        return HeadingToHtml("h6", heading);
    }
}