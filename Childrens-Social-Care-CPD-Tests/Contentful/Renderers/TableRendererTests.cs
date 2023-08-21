using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using TableRenderer = Childrens_Social_Care_CPD.Contentful.Renderers.TableRenderer;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class TableRendererTests
{
    private IRenderer<TableHeader> _tableHeaderRenderer;
    private IRenderer<TableCell> _tableCellRenderer;
    private TableRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _tableHeaderRenderer = Substitute.For<IRenderer<TableHeader>>();
        _tableCellRenderer = Substitute.For<IRenderer<TableCell>>();
        _sut = new TableRenderer(_tableHeaderRenderer, _tableCellRenderer);
    }

    [Test]
    public void Empty_Table_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var table = new Table()
        {
            Content = new List<IContent>()
        };

        _tableHeaderRenderer.Render(Arg.Any<TableHeader>()).Returns(new HtmlString("AAA"));
        _tableCellRenderer.Render(Arg.Any<TableCell>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(table);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<table class=\"HtmlEncode[[govuk-table]]\"></table>");
    }

    [Test]
    public void Table_Renders_Header()
    {
        // arrange
        var stringWriter = new StringWriter();
        var table = new Table()
        {
            Content = new List<IContent>
            {
                new TableRow
                {
                    Content = new List<IContent>
                    {
                        new TableHeader()
                    }
                }
            }
        };
        _tableHeaderRenderer.Render(Arg.Any<TableHeader>()).Returns(new HtmlString("AAA"));
        _tableCellRenderer.Render(Arg.Any<TableCell>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(table);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<table class=\"HtmlEncode[[govuk-table]]\"><thead class=\"HtmlEncode[[govuk-table__head]]\"><tr class=\"HtmlEncode[[govuk-table__row]]\">AAA</tr></thead></table>");
    }

    [Test]
    public void Table_Does_Not_Render_Header_With_No_Content()
    {
        // arrange
        var stringWriter = new StringWriter();
        var table = new Table()
        {
            Content = new List<IContent>
            {
                new TableRow
                {
                    Content = new List<IContent>()
                }
            }
        };
        _tableHeaderRenderer.Render(Arg.Any<TableHeader>()).Returns(new HtmlString("AAA"));
        _tableCellRenderer.Render(Arg.Any<TableCell>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(table);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<table class=\"HtmlEncode[[govuk-table]]\"></table>");
    }

    [Test]
    public void Table_Renders_Body()
    {
        // arrange
        var stringWriter = new StringWriter();
        var table = new Table()
        {
            Content = new List<IContent>
            {
                new TableRow
                {
                    Content = new List<IContent>
                    {
                        new TableCell()
                    }
                }
            }
        };

        _tableHeaderRenderer.Render(Arg.Any<TableHeader>()).Returns(new HtmlString("AAA"));
        _tableCellRenderer.Render(Arg.Any<TableCell>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(table);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<table class=\"HtmlEncode[[govuk-table]]\"><tbody class=\"HtmlEncode[[govuk-table__body]]\"><tr class=\"HtmlEncode[[govuk-table__row]]\">BBB</tr></tbody></table>");
    }

    [Test]
    public void Table_Does_Not_Render_Body_With_No_Content()
    {
        // arrange
        var stringWriter = new StringWriter();
        var table = new Table()
        {
            Content = new List<IContent>
            {
                new TableRow
                {
                    Content = new List<IContent>()
                }
            }
        };

        _tableHeaderRenderer.Render(Arg.Any<TableHeader>()).Returns(new HtmlString("AAA"));
        _tableCellRenderer.Render(Arg.Any<TableCell>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(table);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<table class=\"HtmlEncode[[govuk-table]]\"></table>");
    }
}
