using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using TableCellRenderer = Childrens_Social_Care_CPD.Contentful.Renderers.TableCellRenderer;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class TableCellRendererTests
{
    private IRenderer<Paragraph> _paragraphRenderer;
    private TableCellRenderer _sut;

    [SetUp]
    public void Setup()
    { 
        _paragraphRenderer = Substitute.For<IRenderer<Paragraph>>();
        _sut = new TableCellRenderer(_paragraphRenderer);
    }

    [Test]
    public void TableCell_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph();
        var tableCell = new TableCell()
        {
            Content = new List<IContent> { paragraph }
        };
        _paragraphRenderer.Render(paragraph).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(tableCell);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<td class=\"HtmlEncode[[govuk-table__cell]]\">AAA</td>");
    }
}
