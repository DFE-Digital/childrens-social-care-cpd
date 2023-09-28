using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

[TestFixture]
public partial class PartialsFactoryTests
{
    public static object[] Successful_Resolves =
    {
        new object[] { new AreaOfPractice(), "_AreaOfPractice" },
        new object[] { new AreaOfPracticeList(), "_AreaOfPracticeList" },
        new object[] { new ColumnLayout(), "_ColumnLayout" },
        new object[] { new Content(), "_Content" },
        new object[] { new ContentLink(), "_ContentLink" },
        new object[] { new ContentSeparator(), "_ContentSeparator" },
        new object[] { new DetailedPathway(), "_DetailedPathway" },
        new object[] { new DetailedRole(), "_DetailedRole" },
        new object[] { new HeroBanner(), string.Empty },
        new object[] { new LinkCard(), "_LinkCard" },
        new object[] { new ImageCard(), "_ImageCard" },
        new object[] { new LinkListCard(), "_LinkListCard" },
        new object[] { new Resource(), "_Resource" },
        new object[] { new RichTextBlock(), "_RichTextBlock" },
        new object[] { new RoleList(), "_RoleList" },
        new object[] { new SideMenu(), "_SideMenu" },
        new object[] { new TextBlock(), "_TextBlock" },
        new object[] { new Video(), "_Video" },
    };

    [TestCaseSource(nameof(Successful_Resolves))]
    public void Resolves_Correctly(IContent item, string expectedPartialName)
    {
        var actual = PartialsFactory.GetPartialFor(item);

        actual.Should().Be(expectedPartialName);
    }

    public static IContent[] Unsuccessful_Resolves =
    {
        new TestingContentItem(),
        null
    };

    [TestCaseSource(nameof(Unsuccessful_Resolves))]
    public void Does_Not_Resolve_Unknown_Content(IContent item)
    {
        var expectedPartialName = "_UnknownContentWarning";

        var actual = PartialsFactory.GetPartialFor(item);

        actual.Should().Be(expectedPartialName);
    }
}
