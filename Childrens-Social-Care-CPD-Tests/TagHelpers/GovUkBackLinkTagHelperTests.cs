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

public class GovUkBackLinkTagHelperTests
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
        _tagHelperOutput = new TagHelperOutput("govuk-back-link", new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Output_Is_An_Anchor_Element()
    {
        // arrange
        var tagHelper = new GovUkBackLinkTagHelper();
        _tagHelperOutput.TagMode = TagMode.SelfClosing;

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("a");
    }

    [Test]
    public async Task Adds_Default_Content_When_Input_Tag_Is_Closed()
    {
        // arrange
        var tagHelper = new GovUkBackLinkTagHelper();
        _tagHelperOutput.TagMode = TagMode.SelfClosing;

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.Content.GetContent().Should().Be("Back");
    }

    [Test]
    public async Task Uses_Tag_Content_When_Input_Tag_Is_Open()
    {
        // arrange
        var content = "A String";
        Task<TagHelperContent> func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(content);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
        var tagHelperOutput = new TagHelperOutput("govuk-back-link", new TagHelperAttributeList(), func)
        {
            TagMode = TagMode.StartTagAndEndTag
        };
        var tagHelper = new GovUkBackLinkTagHelper();

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, tagHelperOutput);

        // assert
        tagHelperOutput.Content.GetContent().Should().Be(content);
    }

    [TestCase(false, "govuk-back-link")]
    [TestCase(true, "govuk-back-link-inverse")]
    public async Task Adds_Correct_Css_Class(bool inverse, string expected)
    {
        // arrange
        var tagHelper = new GovUkBackLinkTagHelper
        {
            Inverse = inverse
        };

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var classAttribute = _tagHelperOutput.Attributes.FirstOrDefault(x => x.Name == "class");

        // assert
        classAttribute.Should().NotBeNull();
        classAttribute.Value.Should().Be(expected);
    }

    [Test]
    public async Task Css_Classes_Should_Be_Merged()
    {
        // arrange
        var tagHelper = new GovUkBackLinkTagHelper();
        _tagHelperOutput.Attributes.Add(new TagHelperAttribute("class", "foo", HtmlAttributeValueStyle.DoubleQuotes));

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var classAttribute = _tagHelperOutput.Attributes.FirstOrDefault(x => x.Name == "class");

        // assert
        classAttribute.Should().NotBeNull();
        classAttribute.Value.ToString().Should().Be("foo govuk-back-link");
    }

    [Test]
    public async Task Attributes_Should_Be_Preserved()
    {
        // arrange
        var tagHelper = new GovUkBackLinkTagHelper();
        _tagHelperOutput.Attributes.Add(new TagHelperAttribute("foo", "bar", HtmlAttributeValueStyle.DoubleQuotes));

        // act
        await tagHelper.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var classAttribute = _tagHelperOutput.Attributes.FirstOrDefault(x => x.Name == "foo");

        // assert
        classAttribute.Should().NotBeNull();
        classAttribute.Value.Should().Be("bar");
    }
}
