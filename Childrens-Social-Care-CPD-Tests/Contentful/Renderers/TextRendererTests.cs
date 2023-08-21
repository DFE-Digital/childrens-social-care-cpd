using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using TextRenderer = Childrens_Social_Care_CPD.Contentful.Renderers.TextRenderer;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class TextRendererTests
{
    private TextRenderer _sut = new TextRenderer();

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
        var htmlContent = _sut.Render(text);
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
        var htmlContent = _sut.Render(text);
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
        var htmlContent = _sut.Render(text);
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
        var htmlContent = _sut.Render(text);
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
        var htmlContent = _sut.Render(text);

        // assert
        htmlContent.Should().BeNull();
    }
}
