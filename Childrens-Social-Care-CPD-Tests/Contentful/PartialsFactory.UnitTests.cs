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
        new object[] { new ColumnLayout(), "./ColumnLayout" },
        new object[] { new Content(), "./Content" },
        new object[] { new ContentLink(), "./ContentLink" },
        new object[] { new ContentSeparator(), "./ContentSeparator" },
        new object[] { new DetailedRole(), "./DetailedRole" },
        new object[] { new HeroBanner(), string.Empty },
        new object[] { new LinkCard(), "./LinkCard" },
        new object[] { new LinkListCard(), "./LinkListCard" },
        new object[] { new RichTextBlock(), "./RichTextBlock" },
        new object[] { new RoleList(), "./RoleList" },
        new object[] { new SideMenu(), "./SideMenu" },
        new object[] { new TextBlock(), "./TextBlock" },
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
        var expectedPartialName = "./UnknownContentWarning";

        var actual = PartialsFactory.GetPartialFor(item);

        actual.Should().Be(expectedPartialName);
    }
}
