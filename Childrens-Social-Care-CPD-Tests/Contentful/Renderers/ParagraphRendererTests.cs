using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.IO;
using ParagraphRenderer = Childrens_Social_Care_CPD.Contentful.Renderers.ParagraphRenderer;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class ParagraphRendererTests
{
    private IRenderer<Text> _textRenderer;
    private IRenderer<RoleList> _roleListRenderer;
    private IRenderer<Hyperlink> _hyperlinkRenderer;
    private IRenderer<ContentLink> _contentLinkRenderer;
    private IRenderer<AreaOfPracticeList> _areaOfPracticeListRenderer;

    private ParagraphRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _textRenderer = Substitute.For<IRenderer<Text>>();
        _roleListRenderer = Substitute.For<IRenderer<RoleList>>();
        _hyperlinkRenderer = Substitute.For<IRenderer<Hyperlink>>();
        _contentLinkRenderer = Substitute.For<IRenderer<ContentLink>>();
        _areaOfPracticeListRenderer = Substitute.For<IRenderer<AreaOfPracticeList>>();
        _sut = new ParagraphRenderer(_textRenderer, _roleListRenderer, _hyperlinkRenderer, _contentLinkRenderer, _areaOfPracticeListRenderer);
    }

    [Test]
    public void Paragraph_Renders_With_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            { 
                new Text()
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));

        // act
        var htmlContent = _sut.Render(paragraph);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<p class=\"HtmlEncode[[govuk-body-m]]\">AAA</p>");
    }

    [Test]
    public void Paragraph_Renders_With_RoleList()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            {
                new EntryStructure
                { 
                    Data = new EntryStructureData
                    {
                        Target = new RoleList()
                    }
                }
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));
        _areaOfPracticeListRenderer.Render(Arg.Any<AreaOfPracticeList>()).Returns(new HtmlString("EEE"));

        // act
        var htmlContent = _sut.Render(paragraph);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<p class=\"HtmlEncode[[govuk-body-m]]\">BBB</p>");
    }

    [Test]
    public void Paragraph_Renders_With_Hyperlink()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            {
                new Hyperlink()
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));
        _areaOfPracticeListRenderer.Render(Arg.Any<AreaOfPracticeList>()).Returns(new HtmlString("EEE"));

        // act
        var htmlContent = _sut.Render(paragraph);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<p class=\"HtmlEncode[[govuk-body-m]]\">CCC</p>");
    }

    [Test]
    public void Paragraph_Renders_With_ContentLink()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            {
                new EntryStructure
                {
                    Data = new EntryStructureData
                    {
                        Target = new ContentLink()
                    }
                }
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));
        _areaOfPracticeListRenderer.Render(Arg.Any<AreaOfPracticeList>()).Returns(new HtmlString("EEE"));

        // act
        var htmlContent = _sut.Render(paragraph);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<p class=\"HtmlEncode[[govuk-body-m]]\">DDD</p>");
    }

    [Test]
    public void Paragraph_Renders_With_AreaOfPracticeList()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            {
                new EntryStructure
                {
                    Data = new EntryStructureData
                    {
                        Target = new AreaOfPracticeList()
                    }
                }
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));
        _areaOfPracticeListRenderer.Render(Arg.Any<AreaOfPracticeList>()).Returns(new HtmlString("EEE"));

        // act
        var htmlContent = _sut.Render(paragraph);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<p class=\"HtmlEncode[[govuk-body-m]]\">EEE</p>");
    }

    [Test]
    public void Paragraph_Renders_Inline_If_Options_Set()
    {
        // arrange
        var stringWriter = new StringWriter();
        var paragraph = new Paragraph()
        {
            Content = new List<IContent>
            {
                new Hyperlink()
            }
        };

        _textRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _roleListRenderer.Render(Arg.Any<RoleList>()).Returns(new HtmlString("BBB"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("CCC"));
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("DDD"));
        _areaOfPracticeListRenderer.Render(Arg.Any<AreaOfPracticeList>()).Returns(new HtmlString("EEE"));

        var rendererOptions = new RendererOptions("", "inline");

        // act
        var htmlContent = _sut.Render(paragraph, rendererOptions);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<span>CCC</span>");
    }
}
