using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class Heading2RendererTests
{
    private IRenderer<Text> _textRenderer;
    private IRenderer<Hyperlink> _hyperlinkRenderer;
    private IRenderer<ContentLink> _contentLinkRenderer;
    private Heading2Renderer _sut;

    [SetUp]
    public void Setup()
    {
        _textRenderer = Substitute.For<IRenderer<Text>>();
        _hyperlinkRenderer = Substitute.For<IRenderer<Hyperlink>>();
        _contentLinkRenderer = Substitute.For<IRenderer<ContentLink>>();
        _sut = new Heading2Renderer(_textRenderer, _hyperlinkRenderer, _contentLinkRenderer);
    }

    [Test]
    public void Heading2_Renders()
    {
        // arrange
        var theText = new Text()
        {
            Value = "Foo",
            Marks = new List<Mark>()
        };
        var heading2 = new Heading2()
        {
            Content = new List<IContent> { theText }
        };

        _textRenderer.Render(theText).Returns(new HtmlString("AAA"));
        var stringWriter = new StringWriter();

        // act
        var htmlContent = _sut.Render(heading2);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>AAA</h2>");
    }

    [Test]
    public void Heading2_Renders_Hyperlink()
    {
        // arrange
        var stringWriter = new StringWriter();
        var foo = new Hyperlink()
        {
            Data = new HyperlinkData
            {
                Title = "foo",
                Uri = "bar"
            }
        };
        var heading2 = new Heading2()
        {
            Content = new List<IContent> { foo }
        };

        _hyperlinkRenderer.Render(foo).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(heading2);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>AAA</h2>");
    }

    [Test]
    public void Heading2_Renders_ContentLink()
    {
        // arrange
        var stringWriter = new StringWriter();
        var contentLink = new ContentLink();

        var heading2 = new Heading2()
        {
            Content = new List<IContent> {
                new EntryStructure
                {
                    Data = new EntryStructureData
                    {
                        Target = contentLink
                    }
                }
            }
        };

        _contentLinkRenderer.Render(contentLink).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(heading2);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>AAA</h2>");
    }
}
