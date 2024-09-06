using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

[TestFixture]
public partial class PartialsFactoryTests
{
    private static readonly object[] Successful_Resolves =
    {
        new object[] { new Accordion(), "_Accordion" },
        new object[] { new AccordionSection(), "_AccordionSection" },
        new object[] { new AreaOfPractice(), "_AreaOfPractice" },
        new object[] { new AreaOfPracticeList(), "_AreaOfPracticeList" },
        new object[] { new AudioResource(), "_AudioResource" },
        new object[] { new BackToTop(), "_BackToTop" },
        new object[] { new ColumnLayout(), "_ColumnLayout" },
        new object[] { new Content(), "_Content" },
        new object[] { new ContentLink(), "_ContentLink" },
        new object[] { new ContentSeparator(), "_ContentSeparator" },
        new object[] { new ContentsAnchor(), "_ContentsAnchor" },
        new object[] { new DetailedPathway(), "_DetailedPathway" },
        new object[] { new DetailedRole(), "_DetailedRole" },
        new object[] { new Feedback(), "_Feedback" },
        new object[] { new HeroBanner(), string.Empty },
        new object[] { new LinkCard(), "_LinkCard" },
        new object[] { new ImageCard(), "_ImageCard" },
        new object[] { new NavigationMenu(), "_NavigationMenu" },
        new object[] { new LinkListCard(), "_LinkListCard" },
        new object[] { new PageContents(), "_PageContents" },
        new object[] { new PageContentsItem(), "_PageContentsItem" },
        new object[] { new PdfFileResource(), "_PdfFileResource" },
        new object[] { new RichTextBlock(), "_RichTextBlock" },
        new object[] { new RoleList(), "_RoleList" },
        new object[] { new TextBlock(), "_TextBlock" },
        new object[] { new VideoResource(), "_VideoResource" },
        new object[] { new InfoBox(), "_InfoBox" },
    };

    [TestCaseSource(nameof(Successful_Resolves))]
    public void Resolves_Correctly(IContent item, string expectedPartialName)
    {
        var actual = PartialsFactory.GetPartialFor(item);

        actual.Should().Be(expectedPartialName);
    }

    private static readonly IContent[] Unsuccessful_Resolves =
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
