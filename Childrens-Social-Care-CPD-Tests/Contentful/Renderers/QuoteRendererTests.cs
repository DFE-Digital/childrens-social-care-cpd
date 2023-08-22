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

public class QuoteRendererTests
{
    private IRenderer<Paragraph> _paragraphRenderer;
    private QuoteRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _paragraphRenderer = Substitute.For<IRenderer<Paragraph>>();
        _sut = new QuoteRenderer(_paragraphRenderer);
    }

    [Test]
    public void Quote_Renders_Single_Paragraph()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph();
        var quote = new Quote()
        {
            Content = new List<IContent> { paragraph }
        };
        _paragraphRenderer.Render(paragraph).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(quote);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<div class=\"HtmlEncode[[govuk-inset-text]]\">AAA</div>");
    }

    [Test]
    public void Quote_Ignores_Empty_Paragraphs()
    {
        // arrange
        var stringWriter = new StringWriter();
        
        var quote = new Quote()
        {
            Content = new List<IContent> { null }
        };
        _paragraphRenderer.Render(Arg.Any<Paragraph>()).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(quote);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<div class=\"HtmlEncode[[govuk-inset-text]]\"></div>");
    }

    [Test]
    public void Quote_Renders_Multiple_Paragraphs()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph();
        var quote = new Quote()
        {
            Content = new List<IContent> { paragraph, paragraph }
        };
        _paragraphRenderer.Render(paragraph).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(quote);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<div class=\"HtmlEncode[[govuk-inset-text]]\">AAAAAA</div>");
    }
}
