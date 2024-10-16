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
using List = Contentful.Core.Models.List;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class ListRendererTests
{
    private IRenderer<Text> _textLinkRenderer;
    private IRenderer<Hyperlink> _hyperlinkRenderer;

    private ListRenderer _sut;

    [SetUp]
    public void Setup()
    {
        _textLinkRenderer = Substitute.For<IRenderer<Text>>();
        _hyperlinkRenderer = Substitute.For<IRenderer<Hyperlink>>();
        _sut = new ListRenderer(_textLinkRenderer, _hyperlinkRenderer);
    }

    [Test]
    public void List_Does_Not_Render_When_No_Items()
    {
        // arrange
        var stringWriter = new StringWriter();
        var list = new List()
        {
            Content = new List<IContent>()
        };
        _textLinkRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(list);

        // assert
        htmlContent.Should().BeNull();
    }

    [Test]
    public void Unordered_List_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var list = new List()
        {
            NodeType = "unordered-list",
            Content = new List<IContent>
            {
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Paragraph
                        {
                            Content = new List<IContent> { new Text() }
                        }
                    }
                },
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Paragraph
                        {
                            Content = new List<IContent> { new Hyperlink() }
                        }
                    }

                }
            }
        };
        _textLinkRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(list);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<ul class=\"HtmlEncode[[govuk-list govuk-list--bullet]]\"><li>AAA</li><li>BBB</li></ul>");
    }

    [Test]
    public void Ordered_List_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var list = new List()
        {
            NodeType = "ordered-list",
            Content = new List<IContent>
            {
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Paragraph
                        {
                            Content = new List<IContent> { new Text() }
                        }
                    }
                },
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Paragraph
                        {
                            Content = new List<IContent> { new Hyperlink() }
                        }
                    }

                }
            }
        };
        _textLinkRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(list);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<ol class=\"HtmlEncode[[govuk-list govuk-list--number]]\"><li>AAA</li><li>BBB</li></ol>");
    }

    [Test]
    public void List_Only_Renders_Paragraphs()
    {
        // arrange
        var stringWriter = new StringWriter();
        var list = new List()
        {
            NodeType = "unordered-list",
            Content = new List<IContent>
            {
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Paragraph
                        {
                            Content = new List<IContent> { new Text() }
                        }
                    }
                },
                new ListItem
                {
                    Content = new List<IContent>
                    {
                        new Text()
                    }

                }
            }
        };
        _textLinkRenderer.Render(Arg.Any<Text>()).Returns(new HtmlString("AAA"));
        _hyperlinkRenderer.Render(Arg.Any<Hyperlink>()).Returns(new HtmlString("BBB"));

        // act
        var htmlContent = _sut.Render(list);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<ul class=\"HtmlEncode[[govuk-list govuk-list--bullet]]\"><li>AAA</li></ul>");
    }

}
