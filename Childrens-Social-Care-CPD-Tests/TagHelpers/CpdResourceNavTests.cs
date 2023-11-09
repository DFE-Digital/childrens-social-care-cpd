using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.TagHelpers;

public class CpdResourceNavTests
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
        _tagHelperOutput = new TagHelperOutput(CpdResourceNav.TagName, new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Output_Is_Suppressed_When_Navigation_Not_Set()
    {
        // arrange
        var sut = new CpdResourceNav();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_Suppressed_When_No_Navigation_Items()
    {
        // arrange
        var sut = new CpdResourceNav
        {
            Navigation = new List<ContentLink>()
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_A_Nav_Element()
    {
        // arrange
        var sut = new CpdResourceNav
        {
            Navigation = new List<ContentLink>
            {
                new ContentLink { Uri = "/foo" }
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("nav");
    }

    [TestCase("role", "navigation")]
    public async Task Nav_Has_Attributes(string name, string value)
    {
        // arrange
        var sut = new CpdResourceNav
        {
            Navigation = new List<ContentLink>
            {
                new ContentLink { Uri = "/foo" }
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.Attributes.ContainsName(name);
        _tagHelperOutput.Attributes[name].Value.Should().Be(value);
    }

    [Test]
    public async Task Selected_Item_Is_Rendered()
    {
        // arrange
        var sut = new CpdResourceNav
        {
            Selected = "foo1",
            Navigation = new List<ContentLink>
            {
                new ContentLink { Name = "Foo1", Uri = "/foo1" },
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().NotContain("<a ");
        actual.Should().Contain("<li");
        actual.Should().Contain("aria-current=\"HtmlEncode[[true]]\"");
        actual.Should().Contain("HtmlEncode[[Foo1]]</li>");
    }

    [Test]
    public async Task UnSelected_Item_Is_Rendered()
    {
        // arrange
        var sut = new CpdResourceNav
        {
            Selected = "foo2",
            Navigation = new List<ContentLink>
            {
                new ContentLink { Name = "Foo1", Uri = "/foo1" },
            }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("<a ");
        actual.Should().NotContain("aria-current=\"HtmlEncode[[true]]\"");
        actual.Should().Contain("href=\"HtmlEncode[[/foo1]]\"");
    }
}