using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.TagHelpers;

public class CpdResourcePageNavTests
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
        _tagHelperOutput = new TagHelperOutput(CpdResourcePageNav.TagName, new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Current_Is_Required()
    {
        // arrange
        var sut = new CpdResourcePageNav();
        Func<Task> act = () => sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // act/assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Output_Is_Suppressed_When_Navigation_Not_Set()
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo" };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_Suppressed_When_No_Navigation_Items()
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo" };
        sut.Navigation = new List<ContentLink>();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }

    [Test]
    public async Task Output_Is_A_Nav_Element()
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo" };
        sut.Navigation = new List<ContentLink>
        {
            new ContentLink { Uri = "/foo" }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("nav");
    }

    [TestCase("role", "navigation")]
    [TestCase("aria-label", "pagination")]
    public async Task Nav_Has_Attributes(string name, string value)
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo" };
        sut.Navigation = new List<ContentLink>
        {
            new ContentLink { Uri = "/foo" }
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.Attributes.ContainsName(name);
        _tagHelperOutput.Attributes[name].Value.Should().Be(value);
    }

    [Test]
    public async Task Renders_Only_Next_When_At_Start_Of_Navigation()
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo1" };
        sut.Navigation = new List<ContentLink>
        {
            new ContentLink { Uri = "/foo1" },
            new ContentLink { Uri = "/foo2" },
            new ContentLink { Uri = "/foo3" },
            new ContentLink { Uri = "/foo4" },
            new ContentLink { Uri = "/foo5" },
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().NotContain("prev");
        actual.Should().Contain("next");
    }

    [TestCase("foo2")]
    [TestCase("foo3")]
    [TestCase("foo4")]
    public async Task Renders_Only_Prev_And_Next_When_In_Middle_Of_Navigation(string current)
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = current };
        sut.Navigation = new List<ContentLink>
        {
            new ContentLink { Uri = "/foo1" },
            new ContentLink { Uri = "/foo2" },
            new ContentLink { Uri = "/foo3" },
            new ContentLink { Uri = "/foo4" },
            new ContentLink { Uri = "/foo5" },
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("prev");
        actual.Should().Contain("next");
    }

    [Test]
    public async Task Renders_Only_Prev_When_At_End_Of_Navigation()
    {
        // arrange
        var sut = new CpdResourcePageNav { Current = "foo5" };
        sut.Navigation = new List<ContentLink>
        {
            new ContentLink { Uri = "/foo1" },
            new ContentLink { Uri = "/foo2" },
            new ContentLink { Uri = "/foo3" },
            new ContentLink { Uri = "/foo4" },
            new ContentLink { Uri = "/foo5" },
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("prev");
        actual.Should().NotContain("next");
    }
}