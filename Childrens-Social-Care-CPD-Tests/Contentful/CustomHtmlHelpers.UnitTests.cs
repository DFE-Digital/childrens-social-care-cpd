using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

public class CustomHtmlHelpersTests
{
    [Test]
    public void ContentLink_Returns_Null_When_Not_Provided_A_HtmlHelper()
    {
        var actual = CustomHtmlHelpers.ContentLink(null, string.Empty, string.Empty);

        actual.Should().BeNull();
    }

    [Test]
    public void ContentLink_Builds_Internal_Content_Link_Correctly()
    {
        // arrange
        var uri = "test_path";
        var text = "Link text";
        var expectedUri = $"<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[/content/{uri}]]\">HtmlEncode[[{text}]]</a>";
        var stringWriter = new StringWriter();
        var helper = Substitute.For<IHtmlHelper>();

        // act
        var result = helper.ContentLink(text, uri);
        result.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(expectedUri);
    }

    [Test]
    public void ContentLink_Builds_Internal_Link_Correctly()
    {
        // arrange
        var uri = "/test_path";
        var text = "Link text";
        var expectedUri = $"<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[{uri}]]\">HtmlEncode[[{text}]]</a>";
        var stringWriter = new StringWriter();
        var helper = Substitute.For<IHtmlHelper>();

        // act
        var result = helper.ContentLink(text, uri);
        result.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(expectedUri);
    }

    [Test]
    public void ContentLink_Builds_External_Link_Correctly()
    {
        // arrange
        var uri = "https://www.gov.uk/";
        var text = "Link text";
        var expectedUri = $"<a class=\"HtmlEncode[[govuk-link]]\" href=\"HtmlEncode[[{uri}]]\">HtmlEncode[[{text}]]</a>";
        var stringWriter = new StringWriter();
        var helper = Substitute.For<IHtmlHelper>();

        // act
        var result = helper.ContentLink(text, uri);
        result.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(expectedUri);
    }

    [Test]
    public async Task RenderContentfulPartialAsync_Does_Not_Call_RenderPartial_When_Content_Item_Is_Null()
    {
        // arrange
        var helper = Substitute.For<IHtmlHelper>();

        // act
        await helper.RenderContentfulPartialAsync(null);

        // assert
        await helper.DidNotReceiveWithAnyArgs().RenderPartialAsync(Arg.Any<string>(), Arg.Any<IContent>(), Arg.Any<ViewDataDictionary>());
    }

    [Test]
    public async Task RenderContentfulPartialAsync_Calls_RenderPartial_With_Correct_Parameters()
    {
        // arrange
        var helper = Substitute.For<IHtmlHelper>();
        var textBlock = new TextBlock();
        var partialName = "_TextBlock";

        // act
        await helper.RenderContentfulPartialAsync(textBlock);

        // assert
        await helper.Received().RenderPartialAsync(partialName, textBlock, null);
    }

    [Test]
    public async Task RenderContentfulPartialAsync_Does_Not_Call_RenderPartial_When_Content_Item_Is_HeroBanner()
    {
        // arrange
        var helper = Substitute.For<IHtmlHelper>();
        var heroBanner = new HeroBanner();

        // act
        await helper.RenderContentfulPartialAsync(heroBanner);

        // assert
        await helper.DidNotReceiveWithAnyArgs().RenderPartialAsync(Arg.Any<string>(), Arg.Any<IContent>(), Arg.Any<ViewDataDictionary>());
    }
}
