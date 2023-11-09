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

public class CpdPropertiesListTests
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
        _tagHelperOutput = new TagHelperOutput(CpdPropertiesList.TagName, new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Output_Is_Suppressed_When_Items_Not_Set()
    {
        // arrange
        var sut = new CpdPropertiesList();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_Suppressed_When_No_Items()
    {
        // arrange
        var sut = new CpdPropertiesList
        {
            Items = new Dictionary<string, string>()
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_A_UL_Element()
    {
        // arrange
        var sut = new CpdPropertiesList
        {
            Items = new Dictionary<string, string>() { { "Foo", "foo" } }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("ul");
    }

    [Test]
    public async Task Items_Are_Rendered()
    {
        // arrange
        var sut = new CpdPropertiesList
        {
            Items = new Dictionary<string, string>()
            {
                { "Foo", "foo" },
                { "Foo2", "foo2" }
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("Foo");
        actual.Should().Contain("Foo2");
    }

    [Test]
    public async Task Items_Are_Rendered_As_ListItem()
    {
        // arrange
        var sut = new CpdPropertiesList
        {
            Items = new Dictionary<string, string>()
            {
                { "Foo", "foo" }
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("<li><span class=\"HtmlEncode[[govuk-!-font-size-16]]\">HtmlEncode[[Foo");
    }
}