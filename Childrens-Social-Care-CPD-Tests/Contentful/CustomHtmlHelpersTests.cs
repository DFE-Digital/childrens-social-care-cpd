using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

public class CustomHtmlHelpersTests
{
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
