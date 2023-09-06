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
using System.Text;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class RelatedContentRendererTests
{
    private IRenderer<ContentLink> _contentLinkRenderer;
    private RelatedContentRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _contentLinkRenderer = Substitute.For<IRenderer<ContentLink>>();
        _sut = new RelatedContentRenderer(_contentLinkRenderer);
    }

    [Test]
    public void RelatedContent_Returns_Null_When_Passed_Null()
    {
        // act
        var htmlContent = _sut.Render(null);

        // assert
        htmlContent.Should().BeNull();
    }

    [Test]
    public void RelatedContent_Renders_Title()
    {
        // arrange
        var stringWriter = new StringWriter();
        var relatedContent = new RelatedContent();

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        var expected = $".*?{Regex.Escape("<h2 class=\"HtmlEncode[[govuk-heading-s]]\" id=\"HtmlEncode[[related-nav__section]]\">HtmlEncode[[Related content]]</h2>")}.*";

        // act
        var htmlContent = _sut.Render(relatedContent);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void RelatedContent_Renders_Navigation()
    {
        // arrange
        var sb = new StringBuilder();
        var stringWriter = new StringWriter();
        var relatedContent = new RelatedContent
        {
            new ContentLink { Name = "foo", Uri = "bar" },
            new ContentLink { Name = "foo", Uri = "bar" },
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        sb.Append("<nav aria-labelledby=\"HtmlEncode[[related-nav__section]]\" role=\"HtmlEncode[[navigation]]\">");
        sb.Append("<ul class=\"HtmlEncode[[govuk-list govuk-!-font-size-16]]\">");
        sb.Append("<li>AAA</li>");
        sb.Append("<li>AAA</li>");
        sb.Append("</ul></nav>");
        var expected = $".*?{Regex.Escape(sb.ToString())}.*";

        // act
        var htmlContent = _sut.Render(relatedContent);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void RelatedContent_Renders()
    {
        // arrange
        var sb = new StringBuilder();
        var stringWriter = new StringWriter();
        var relatedContent = new RelatedContent
        {
            new ContentLink { Name = "foo", Uri = "bar" },
            new ContentLink { Name = "foo", Uri = "bar" },
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        sb.Append("<div class=\"HtmlEncode[[govuk-grid-row]]\">");
        sb.Append("<div class=\"HtmlEncode[[govuk-grid-column-three-quarters]]\">");
        sb.Append("<div class=\"HtmlEncode[[govuk-!-margin-top-28]]\">");
        sb.Append("<aside class=\"HtmlEncode[[app-related-items]]\" role=\"HtmlEncode[[complementary]]\">");
        var expected = $"^{Regex.Escape(sb.ToString())}.*</aside></div></div></div>";

        // act
        var htmlContent = _sut.Render(relatedContent);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }
}
