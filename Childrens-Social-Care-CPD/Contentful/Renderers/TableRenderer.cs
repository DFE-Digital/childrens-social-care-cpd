using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TableRenderer : IRenderer<Table>
{
    private readonly IRenderer<TableHeader> _tableHeaderRenderer;
    private readonly IRenderer<TableCell> _tableCellRenderer;

    public TableRenderer(IRenderer<TableHeader> tableHeaderRenderer, IRenderer<TableCell> tableCellRenderer)
    {
        _tableHeaderRenderer = tableHeaderRenderer;
        _tableCellRenderer = tableCellRenderer;
    }

    public IHtmlContent Render(Table item)
    {
        var table = new TagBuilder("table");
        table.AddCssClass("govuk-table");

        foreach (TableRow row in item.Content)
        {
            if (row.Content.Any(x => x.GetType() == typeof(TableHeader)))
            {
                var thead = new TagBuilder("thead");
                thead.AddCssClass("govuk-table__head");

                var tr = new TagBuilder("tr");
                tr.AddCssClass("govuk-table__row");

                foreach (TableHeader tableHeader in row.Content)
                {
                    tr.InnerHtml.AppendHtml(_tableHeaderRenderer.Render(tableHeader));
                }

                thead.InnerHtml.AppendHtml(tr);
                table.InnerHtml.AppendHtml(thead);
                break;
            }
        }

        var tbody = new TagBuilder("tbody");
        tbody.AddCssClass("govuk-table__body");

        foreach (TableRow row in item.Content)
        {
            if (row.Content.Any(x => x.GetType() == typeof(TableCell)))
            {
                var tr = new TagBuilder("tr");
                tr.AddCssClass("govuk-table__row");

                foreach (TableCell tableCell in row.Content)
                {
                    tr.InnerHtml.AppendHtml(_tableCellRenderer.Render(tableCell));
                }

                tbody.InnerHtml.AppendHtml(tr);
            }
        }

        table.InnerHtml.AppendHtml(tbody);
        return table;
    }
}
