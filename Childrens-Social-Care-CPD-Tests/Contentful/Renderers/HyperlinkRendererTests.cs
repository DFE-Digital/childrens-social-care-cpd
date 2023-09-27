using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class HyperlinkRendererTests
{
    private readonly HyperlinkRenderer _sut = new();

    [Test]
    public void HyperlinkToHtml_Returns_Anchor()
    {
        // arrange
        var stringWriter = new StringWriter();
        var hyperlink = new Hyperlink()
        {
            Content = new List<IContent> {
                new Text() { Value = "Foo" }
            },
            Data = new HyperlinkData()
            {
                Uri = "Bar"
            }
        };

        // act
        var htmlContent = _sut.Render(hyperlink);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[Bar]]\">HtmlEncode[[Foo]]</a>");
    }

    [Test]
    public void HyperlinkToHtml_Returns_Anchor_With_Empty_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var hyperlink = new Hyperlink()
        {
            Content = new List<IContent>(),
            Data = new HyperlinkData()
            {
                Uri = "Bar"
            }
        };

        // act
        var htmlContent = _sut.Render(hyperlink);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[Bar]]\"></a>");
    }
}
