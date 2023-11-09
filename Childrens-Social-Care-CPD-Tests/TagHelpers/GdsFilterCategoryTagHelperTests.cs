using Childrens_Social_Care_CPD.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.TagHelpers;

public class GdsFilterCategoryTagHelperTests
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
        _tagHelperOutput = new TagHelperOutput("gds-filter-category", new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Output_Is_An_Div_Element()
    {
        // arrange
        var sut = new GdsFilterCategoryTagHelper();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("div");
    }

    [Test]
    public async Task Index_Should_Be_Used()
    {
        // arrange
        var sut = new GdsFilterCategoryTagHelper();
        sut.Index = 3;

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("id=\"HtmlEncode[[accordion-default-heading-3]]\"");
        actual.Should().Contain("id=\"HtmlEncode[[accordion-default-content-3]]\"");
    }

    [Test]
    public async Task Title_Should_Be_Used()
    {
        // arrange
        var sut = new GdsFilterCategoryTagHelper();
        sut.Title = "Foo";

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain(">Foo<");
    }

    [Test]
    public async Task ChildContent_Should_Be_Rendered()
    {
        // arrange
        var sut = new GdsFilterCategoryTagHelper();

        var content = "<child content>";
        Task<TagHelperContent> func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(content);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
        var tagHelperOutput = new TagHelperOutput("gds-filter-category", new TagHelperAttributeList(), func)
        {
            TagMode = TagMode.StartTagAndEndTag
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("<child content>");
    }
}
