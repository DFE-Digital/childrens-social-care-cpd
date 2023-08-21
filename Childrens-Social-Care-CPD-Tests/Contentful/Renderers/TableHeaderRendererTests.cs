using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using TableHeaderRenderer = Childrens_Social_Care_CPD.Contentful.Renderers.TableHeaderRenderer;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class TableHeaderRendererTests
{
    private IRenderer<Text> _textRenderer;
    private IRenderer<Hyperlink> _hyperlinkRenderer;
    private TableHeaderRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _textRenderer = Substitute.For<IRenderer<Text>>();
        _hyperlinkRenderer = Substitute.For<IRenderer<Hyperlink>>();
        _sut = new TableHeaderRenderer(_textRenderer, _hyperlinkRenderer);
    }

    [Test]
    public void TableHeader_Renders_With_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var tableHeader = new TableHeader()
        {
            Content = new List<IContent>
            {
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                    }
                }
            }
        };
        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(tableHeader);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<th class=\"HtmlEncode[[govuk-table__header]]\" scope=\"HtmlEncode[[col]]\">AAA</th>");
    }

    [Test]
    public void TableHeader_Renders_With_Hyperlink()
    {
        // arrange
        var stringWriter = new StringWriter();
        var tableHeader = new TableHeader()
        {
            Content = new List<IContent>
            {
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Hyperlink()
                    }
                }
            }
        };
        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(tableHeader);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<th class=\"HtmlEncode[[govuk-table__header]]\" scope=\"HtmlEncode[[col]]\">BBB</th>");
    }
}
