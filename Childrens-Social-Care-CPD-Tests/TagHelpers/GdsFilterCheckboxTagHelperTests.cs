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

public class GdsFilterCheckboxTagHelperTests
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
        _tagHelperOutput = new TagHelperOutput("gds-filter-checkbox", new TagHelperAttributeList(), func);
    }

    [Test]
    public async Task Output_Is_An_Div_Element()
    {
        // arrange
        var sut = new GdsFilterCheckboxTagHelper();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.TagName.Should().Be("div");
    }

    [Test]
    public async Task Id_Should_Be_Used()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Id = "foo";

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("id=\"HtmlEncode[[foo]]\"");
    }

    [Test]
    public async Task Name_Should_Be_Used()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Name = "foo";

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("name=\"HtmlEncode[[foo]]\"");
    }

    [Test]
    public async Task Value_Should_Be_Used()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Value = "foo";

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("value=\"HtmlEncode[[foo]]\"");
    }

    [Test]
    public async Task Checked_Should_Be_Used()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Checked = true;

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("checked");
    }

    [Test]
    public async Task Checked_Should_Not_Be_Used()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Checked = false;

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().NotContain("checked");
    }

    [Test]
    public async Task Output_Is_A_Checkbox()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();
        sut.Checked = false;

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);
        _tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("type=\"HtmlEncode[[checkbox]]\"");
    }

    [Test]
    public async Task ChildContent_Should_Be_Rendered()
    {
        // arrange
        var stringWriter = new StringWriter();
        var sut = new GdsFilterCheckboxTagHelper();

        var content = "<child content>";
        Task<TagHelperContent> func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(content);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
        var tagHelperOutput = new TagHelperOutput("gds-filter-checkbox", new TagHelperAttributeList(), func)
        {
            TagMode = TagMode.StartTagAndEndTag
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, tagHelperOutput);
        tagHelperOutput.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Contain("<child content>");
    }
}
