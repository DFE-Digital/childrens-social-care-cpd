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

        var headerRow = item.Content.FirstOrDefault(row => (row as TableRow)?.Content.Any(tableRow => tableRow.GetType() == typeof(TableHeader)) ?? false) as TableRow;
        if (headerRow != null)
        {
            var thead = new TagBuilder("thead");
            thead.AddCssClass("govuk-table__head");

            var tr = new TagBuilder("tr");
            tr.AddCssClass("govuk-table__row");

            foreach (TableHeader tableHeader in headerRow.Content)
            {
                tr.InnerHtml.AppendHtml(_tableHeaderRenderer.Render(tableHeader));
            }

            thead.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(thead);
        }

        var bodyRows = item.Content.Where(row => (row as TableRow)?.Content.Any(tableRow => tableRow.GetType() == typeof(TableCell)) ?? false);
        foreach (TableRow row in bodyRows)
        {
            var tbody = new TagBuilder("tbody");
            tbody.AddCssClass("govuk-table__body");

            var tr = new TagBuilder("tr");
            tr.AddCssClass("govuk-table__row");

            foreach (TableCell tableCell in row.Content)
            {
                tr.InnerHtml.AppendHtml(_tableCellRenderer.Render(tableCell));
            }

            tbody.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(tbody);
        }

        return table;
    }
}
