using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TableCellRenderer(IRenderer<Paragraph> paragraphRenderer) : IRenderer<TableCell>
{
    public IHtmlContent Render(TableCell item)
    {
        var td = new TagBuilder("td");
        td.AddCssClass("govuk-table__cell");

        foreach (var content in item.Content)
        {
            switch (content)
            {
                case Paragraph paragraph: td.InnerHtml.AppendHtml(paragraphRenderer.Render(paragraph)); break;
            }
        }

        return td;
    }
}
