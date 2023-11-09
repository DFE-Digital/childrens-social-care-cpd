using Childrens_Social_Care_CPD.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.TagHelpers;

public class GovUkPaginationTagHelperTests
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
        _tagHelperOutput = new TagHelperOutput("govuk-pagination", new TagHelperAttributeList(), func);
    }

    [Test]
    public void Outputs_Nothing_When_No_Pages()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 0
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be(null);
        _tagHelperOutput.Content.IsEmptyOrWhiteSpace.Should().Be(true);
    }

    [Test]
    public void Should_Be_A_Nav()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 1,
            CurrentPage =1,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("nav");
    }


    [TestCase(1)]
    [TestCase(2)]
    public void Should_Highlight_Current_Page(int currentPage)
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = currentPage,
            UrlFormatString = "{0}"
        };
        var regex = new Regex("<a[^>]*>HtmlEncode\\[\\[" + currentPage + "\\]\\]<\\/a>");

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();
        var match = regex.Match(actual);

        // assert
        match.Value.Should().Contain("current");
    }

    [Test]
    public void Should_Not_Highlight_Next_Page()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = 1,
            UrlFormatString = "{0}"
        };
        var regex = new Regex("<a[^>]*>HtmlEncode\\[\\[2\\]\\]<\\/a>");

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();
        var match = regex.Match(actual);

        // assert
        match.Value.Should().NotContain("current");
    }

    [Test]
    public void Shows_Next_Link()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = 1,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("HtmlEncode[[Next]]");
    }

    [Test]
    public void Does_Not_Show_Previous_Link_On_First_Page()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = 1,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().NotContain("HtmlEncode[[Previous]]");
    }

    [Test]
    public void Shows_Previous_Link()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = 2,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("HtmlEncode[[Previous]]");
    }

    [Test]
    public void Does_Not_Show_Next_Link_On_Last_Page()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 2,
            CurrentPage = 2,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().NotContain("HtmlEncode[[Next]]");
    }

    [Test]
    public void Ellipses_Are_Generated()
    {
        // arrange
        var tagHelper = new GovUkPaginationTagHelper
        {
            PageCount = 4,
            CurrentPage = 1,
            UrlFormatString = "{0}"
        };

        // act
        tagHelper.Process(_tagHelperContext, _tagHelperOutput);
        var actual = _tagHelperOutput.AsString();

        // assert
        actual.Should().Contain("&ctdot;");
    }
}
