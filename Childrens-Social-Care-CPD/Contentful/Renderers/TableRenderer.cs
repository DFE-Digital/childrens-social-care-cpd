using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

internal class TableRenderer(IRenderer<TableHeader> tableHeaderRenderer, IRenderer<TableCell> tableCellRenderer) : IRenderer<Table>
{
    public IHtmlContent Render(Table item)
    {
        var table = new TagBuilder("table");
        table.AddCssClass("govuk-table");

        var tableRows = item.Content.OfType<TableRow>();
        var headerRow = tableRows.FirstOrDefault(row => row.Content.Exists(c => c.GetType() == typeof(TableHeader)));
        if (headerRow != null)
        {
            var thead = new TagBuilder("thead");
            thead.AddCssClass("govuk-table__head");

            var tr = new TagBuilder("tr");
            tr.AddCssClass("govuk-table__row");

            foreach (TableHeader tableHeader in headerRow.Content.OfType<TableHeader>())
            {
                tr.InnerHtml.AppendHtml(tableHeaderRenderer.Render(tableHeader));
            }

            thead.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(thead);
        }

        var bodyRows = tableRows.Where(row => row.Content.Exists(tableRow => tableRow.GetType() == typeof(TableCell)));
        foreach (TableRow row in bodyRows)
        {
            var tbody = new TagBuilder("tbody");
            tbody.AddCssClass("govuk-table__body");

            var tr = new TagBuilder("tr");
            tr.AddCssClass("govuk-table__row");

            foreach (TableCell tableCell in row.Content.OfType<TableCell>())
            {
                tr.InnerHtml.AppendHtml(tableCellRenderer.Render(tableCell));
            }

            tbody.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(tbody);
        }

        return table;
    }
}
