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

public class InlineAreaOfPracticeListRendererTests
{
    private IRenderer<ContentLink> _contentLinkRenderer;
    private InlineAreaOfPracticeListRenderer _sut;

    [SetUp]
    public void Setup()
    { 
        _contentLinkRenderer = Substitute.For<IRenderer<ContentLink>>();
        _sut = new InlineAreaOfPracticeListRenderer(_contentLinkRenderer);
    }

    [Test]
    public void AreaOfPractice_Renders_When_No_AreasOfPractice()
    {
        // arrange
        var stringWriter = new StringWriter();
        var areaOfPracticeList = new AreaOfPracticeList()
        {
            Title = "A Title",
            Areas = new List<Content>()
        };
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(areaOfPracticeList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<span>HtmlEncode[[No Areas Of practice Available]]</span>");
    }

    [Test]
    public void AreaOfPracticeList_Renders_Empty_When_AreaOfPractice_Is_Null()
    {
        // arrange
        var stringWriter = new StringWriter();
        var areaOfPracticeList = new AreaOfPracticeList()
        {
            Title = "A Title",
            Areas = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Title = "title",
                    Items = new List<IContent>()
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(areaOfPracticeList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void AreaOfPracticeList_Renders_Title()
    {
        // arrange
        var stringWriter = new StringWriter();
        var areaOfPracticeList = new AreaOfPracticeList
        {
            Title = "A Title",
            Areas = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        new AreaOfPractice()
                        {
                            Title = "title"
                        }
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        var expected = $"^{Regex.Escape("<div class=\"HtmlEncode[[govuk-heading-s govuk-!-margin-bottom-1]]\"><h2>AAA</h2></div>")}.*";

        // act
        var htmlContent = _sut.Render(areaOfPracticeList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void AreaOfPracticeList_Renders_Summary()
    {
        // arrange
        var stringWriter = new StringWriter();
        var areaOfPractice = new AreaOfPractice()
        {
            Title = "title",
            AreaOfPracticeListSummary = "summary"
        };
        var areaOfPracticeList = new AreaOfPracticeList()
        {
            Title = "A Title",
            Areas = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        areaOfPractice
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        var expected = $".*?{Regex.Escape("<p class=\"HtmlEncode[[govuk-body]]\">HtmlEncode[[summary]]</p>")}.*";

        // act
        var htmlContent = _sut.Render(areaOfPracticeList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }
}
