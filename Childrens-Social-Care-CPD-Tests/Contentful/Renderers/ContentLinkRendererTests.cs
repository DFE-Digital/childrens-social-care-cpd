using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class ContentLinkRendererTests
{
    private readonly IRendererWithOptions<ContentLink> _sut = new ContentLinkRenderer();

    [TestCase("http://foo", "http://foo")]
    [TestCase("https://foo", "https://foo")]
    [TestCase("/foo", "/foo")]
    [TestCase("foo", "/foo")]
    public void ContentLink_Renders(string uri, string expectedUri)
    {
        // arrange
        var stringWriter = new StringWriter();
        var contentLink = new ContentLink()
        {
            Name = "Foo",
            Uri = uri
        };

        // act
        var htmlContent = (_sut as IRenderer<ContentLink>).Render(contentLink);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[{expectedUri}]]\">HtmlEncode[[Foo]]</a>");
    }

    [Test]
    public void ContentLink_RenderWithOptions_Adds_Css()
    {
        // arrange
        var stringWriter = new StringWriter();
        var contentLink = new ContentLink()
        {
            Name = "Foo",
            Uri = "/foo"
        };

        // act
        var htmlContent = _sut.Render(contentLink, new RendererOptions(Css: "foo"));
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain($"class=\"HtmlEncode[[govuk-link foo]]\"");
    }
}
