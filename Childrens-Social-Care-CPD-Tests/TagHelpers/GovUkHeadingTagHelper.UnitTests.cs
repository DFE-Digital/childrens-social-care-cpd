using Childrens_Social_Care_CPD.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.TagHelpers;

public class GovUkHeadingTagHelperTests
{
    private TagHelperContext _tagHelperContext;
    private TagHelperOutput _tagHelperOutput;

    [SetUp]
    public void SetUp()
    {
        static Task<TagHelperContent> func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(string.Empty);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }

        _tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "id");
        _tagHelperOutput = new TagHelperOutput("govuk-heading", new TagHelperAttributeList(), func);
    }

    [TestCase(1, "h1")]
    [TestCase(2, "h2")]
    [TestCase(3, "h3")]
    [TestCase(4, "h4")]
    public void GovUkHeading_Generates_Basic_Heading(int level, string expected)
    {
        // arrange
        var tagHelper = new GovUkHeadingTagHelper
        {
            Level = level
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be(expected);
    }

    [Test]
    public void GovUkHeading_Defaults_To_H1()
    {
        // arrange
        var tagHelper = new GovUkHeadingTagHelper();

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("h1");
    }

    [TestCase(0, "h1")]
    [TestCase(5, "h4")]
    public void GovUkHeading_Clamps_Heading_Level(int level, string expected)
    {
        // arrange
        var tagHelper = new GovUkHeadingTagHelper
        {
            Level = level
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be(expected);
    }

    [TestCase(1, "govuk-heading-xl")]
    [TestCase(2, "govuk-heading-l")]
    [TestCase(3, "govuk-heading-m")]
    [TestCase(4, "govuk-heading-s")]
    public void GovUkHeading_Applies_Override_Css(int cssLevel, string expected)
    {
        // arrange
        var tagHelper = new GovUkHeadingTagHelper
        {
            Level = 1,
            DisplaySize = cssLevel
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.Attributes.Single(x => x.Name == "class").Value.Should().Be(expected);
    }
}
