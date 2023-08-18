using Childrens_Social_Care_CPD.Contentful.Models;
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

    public static IHtmlContent ToHtml(this HorizontalRuler horizontalRuler)
    {
        var hr = new TagBuilder("hr")
        {
            TagRenderMode = TagRenderMode.SelfClosing
        };
        hr.AddCssClass("govuk-section-break govuk-section-break--m govuk-section-break--visible");
        return hr;
    }

    public static IHtmlContent ToHtml(this Paragraph contentfulParagraph)
    {
        var p = new TagBuilder("p");
        p.AddCssClass("govuk-body-m");

        foreach (var content in contentfulParagraph.Content)
        {
            switch (content)
            {
                case Text text: p.InnerHtml.AppendHtml(text.ToHtml()); break;
                case EntryStructure entryStructure:
                    {
                        switch (entryStructure.Data.Target)
                        {
                            case RoleList roleList: p.InnerHtml.AppendHtml(roleList.ToHtml()); break;
                        }
                        break;
                    }
                case (Hyperlink hyperlink): p.InnerHtml.AppendHtml(hyperlink.ToHtml()); break;
            }
        }

        return p;
    }

    public static IHtmlContent ToHtml(this Quote quote)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-inset-text");

        foreach (var content in quote.Content)
        {
            var paragraph = content as Paragraph;
            if (paragraph != null)
            {
                div.InnerHtml.AppendHtml(paragraph.ToHtml());
            }
        }

        return div;
    }

    public static IHtmlContent ToHtml(this TableCell tableCell)
    {
        var td = new TagBuilder("td");
        td.AddCssClass("govuk-table__cell");

        foreach (var content in tableCell.Content)
        {
            switch (content)
            {
                case Paragraph paragraph: td.InnerHtml.AppendHtml(paragraph.ToHtml()); break;
            }
        }

        return td;
    }

    public static IHtmlContent ToHtml(this TableHeader tableHeader)
    {
        var th = new TagBuilder("th");
        th.AddCssClass("govuk-table__header");
        th.Attributes.Add("scope", "col");
        
        foreach (var content in tableHeader.Content)
        {
            switch (content)
            {
                case Paragraph paragraph:
                    {
                        foreach (var pContent in paragraph.Content)
                        {
                            switch (pContent)
                            {
                                case Text text: th.InnerHtml.AppendHtml(text.ToHtml()); break;
                                case Hyperlink hyperlink: th.InnerHtml.AppendHtml(hyperlink.ToHtml()); break;
                            }
                        }
                        break;
                    }
            }
        }

        return th;
    }

    public static IHtmlContent ToHtml(this Table contentfulTable)
    {
        var table = new TagBuilder("table");
        table.AddCssClass("govuk-table");

        foreach (TableRow row in contentfulTable.Content)
        {
            if (row.Content.Any(x => x.GetType() == typeof(TableHeader)))
            {
                var thead = new TagBuilder("thead");
                thead.AddCssClass("govuk-table__head");

                var tr = new TagBuilder("tr");
                tr.AddCssClass("govuk-table__row");

                foreach (TableHeader header in @row.Content)
                {
                    tr.InnerHtml.AppendHtml(header.ToHtml());
                }

                thead.InnerHtml.AppendHtml(tr);
                table.InnerHtml.AppendHtml(thead);
                break;
            }
        }

        var tbody = new TagBuilder("tbody");
        tbody.AddCssClass("govuk-table__body");
        
        foreach (TableRow row in contentfulTable.Content)
        {
            if (row.Content.Any(x => x.GetType() == typeof(TableCell)))
            {
                var tr = new TagBuilder("tr");
                tr.AddCssClass("govuk-table__row");

                foreach (TableCell tableCell in @row.Content)
                {
                    tr.InnerHtml.AppendHtml(tableCell.ToHtml());
                }

                tbody.InnerHtml.AppendHtml(tr);
            }
        }

        table.InnerHtml.AppendHtml(tbody);
        return table;
    }

}