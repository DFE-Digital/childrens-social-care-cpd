using Childrens_Social_Care_CPD.Contentful;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

public class ContentfulModelExtensionsTests
{
    #region Text

    [Test]
    public void TextToHtml_Renders_Encoded_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var text = new Text()
        { 
            Value = "Foo",
            Marks = new List<Mark>()
        };

        // act
        var htmlContent = text.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("HtmlEncode[[Foo]]");
    }

    [TestCase("bold", "<strong>HtmlEncode[[Foo]]</strong>")]
    [TestCase("underline", "<u>HtmlEncode[[Foo]]</u>")]
    [TestCase("italic", "<i>HtmlEncode[[Foo]]</i>")]
    [TestCase("code", "<code>HtmlEncode[[Foo]]</code>")]
    [TestCase("superscript", "<sup>HtmlEncode[[Foo]]</sup>")]
    [TestCase("subscript", "<sub>HtmlEncode[[Foo]]</sub>")]
    public void TextToHtml_Renders_Simple_Text_With_Mark(string mark, string expected)
    {
        // arrange
        var stringWriter = new StringWriter();
        var text = new Text()
        {
            Value = "Foo",
            Marks = new List<Mark>() {
                new Mark() { Type = mark }
            }
        };

        // act
        var htmlContent = text.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(expected);
    }

    [Test]
    public void TextToHtml_Renders_Simple_Text_With_Multiple_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var text = new Text()
        {
            Value = "Foo",
            Marks = new List<Mark>() {
                new Mark() { Type = "bold" },
                new Mark() { Type = "underline" },
                new Mark() { Type = "italic" }
            }
        };

        // act
        var htmlContent = text.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<i><u><strong>HtmlEncode[[Foo]]</strong></u></i>");
    }

    [Test]
    public void TextToHtml_Renders_Simple_Text_With_NewLine()
    {
        // arrange
        var stringWriter = new StringWriter();
        var text = new Text()
        {
            Value = "Foo\nBar",
            Marks = new List<Mark>()
        };

        // act
        var htmlContent = text.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("HtmlEncode[[Foo]]<br>HtmlEncode[[Bar]]");
    }

    [TestCase(null)]
    [TestCase("")]
    public void TextToHtml_Returns_Null(string textValue)
    {
        // arrange
        var text = new Text()
        {
            Value = textValue
        };

        // act
        var htmlContent = text.ToHtml();
        
        // assert
        htmlContent.Should().BeNull();
    }

    #endregion

    #region Hyperlink

    [Test]
    public void HyperlinkToHtml_Returns_Anchor()
    {
        // arrange
        var stringWriter = new StringWriter();
        var hyperlink = new Hyperlink()
        {
            Content  = new List<IContent> {
                new Text() { Value = "Foo" }
            },
            Data = new HyperlinkData()
            {
                Uri = "Bar"
            }
        };

        // act
        var htmlContent = hyperlink.ToHtml();
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
        var htmlContent = hyperlink.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[Bar]]\"></a>");
    }

    #endregion

    #region Headings

    #region H1

    [Test]
    public void Heading1_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading1 = new Heading1()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading1.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h1>HtmlEncode[[Foo]]</h1>");
    }

    [Test]
    public void Heading1_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading1 = new Heading1()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading1.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h1>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h1>");
    }

    [Test]
    public void Heading1_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading1 = new Heading1()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                { 
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading1.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h1>HtmlEncode[[Foo]]</h1>");
    }

    #endregion

    #region H2

    [Test]
    public void Heading2_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading2 = new Heading2()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading2.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>HtmlEncode[[Foo]]</h2>");
    }

    [Test]
    public void Heading2_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading2 = new Heading2()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading2.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h2>");
    }

    [Test]
    public void Heading2_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading2 = new Heading2()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading2.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h2>HtmlEncode[[Foo]]</h2>");
    }

    #endregion

    #region H3

    [Test]
    public void Heading3_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading3 = new Heading3()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading3.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h3>HtmlEncode[[Foo]]</h3>");
    }

    [Test]
    public void Heading3_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading3 = new Heading3()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading3.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h3>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h3>");
    }

    [Test]
    public void Heading3_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading3 = new Heading3()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading3.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h3>HtmlEncode[[Foo]]</h3>");
    }

    #endregion

    #region H4

    [Test]
    public void Heading4_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading4 = new Heading4()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading4.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h4>HtmlEncode[[Foo]]</h4>");
    }

    [Test]
    public void Heading4_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading4 = new Heading4()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading4.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h4>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h4>");
    }

    [Test]
    public void Heading4_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading4 = new Heading4()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading4.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h4>HtmlEncode[[Foo]]</h4>");
    }

    #endregion

    #region H5

    [Test]
    public void Heading5_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading5 = new Heading5()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading5.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h5>HtmlEncode[[Foo]]</h5>");
    }

    [Test]
    public void Heading5_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading5 = new Heading5()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading5.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h5>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h5>");
    }

    [Test]
    public void Heading5_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading5 = new Heading5()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading5.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h5>HtmlEncode[[Foo]]</h5>");
    }

    #endregion

    #region H6

    [Test]
    public void Heading6_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading6 = new Heading6()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                }
            }
        };

        // act
        var htmlContent = heading6.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h6>HtmlEncode[[Foo]]</h6>");
    }

    [Test]
    public void Heading6_Renders_With_Marks()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading6 = new Heading6()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Text()
                {
                    Value = "Bold",
                    Marks = new List<Mark>
                    {
                        new Mark() { Type = "bold" }
                    }
                }
            }
        };

        // act
        var htmlContent = heading6.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h6>HtmlEncode[[Foo]]<strong>HtmlEncode[[Bold]]</strong></h6>");
    }

    [Test]
    public void Heading6_Renders_Ignoring_Non_Text()
    {
        // arrange
        var stringWriter = new StringWriter();
        var heading6 = new Heading6()
        {
            Content = new List<IContent>
            {
                new Text()
                {
                    Value = "Foo",
                    Marks = new List<Mark>()
                },
                new Paragraph()
                {
                    Content = new List<IContent>
                    {
                        new Text()
                        {
                            Value = "Bold",
                            Marks = new List<Mark>
                            {
                                new Mark() { Type = "bold" }
                            }
                        }
                    }
                }
            }
        };

        // act
        var htmlContent = heading6.ToHtml();
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<h6>HtmlEncode[[Foo]]</h6>");
    }

    #endregion

    #endregion
}
