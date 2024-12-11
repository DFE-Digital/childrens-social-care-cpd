using System.Threading.Tasks;
using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Navigation;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Navigation;

public class PathwaysNavigationHelperTests
{
    [Test]
    public async Task Page_Of_Type_Pathways_Overview_Page_Not_Associated_With_Pathways_Module_Should_Render_With_Default_Next () {

        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public async Task Page_Of_Type_Pathways_Overview_Page_Associated_With_Pathways_Module_With_No_Contents_Page_Configured_Should_Render_With_Default_Next () {

        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage,
            PathwaysModule = new PathwaysModule()
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }


    [Test]
    public async Task Page_Of_Type_Pathways_Overview_Page_Associated_With_Pathways_Module_With_Contents_Page_Configured_Should_Have_ContentsPage_Id_As_Next_Url () {

        // setup
        var contentsPageId = "CONTENTS_PAGE_ID";
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage,
            PathwaysModule = new PathwaysModule()
            {
                ContentsPage = new Content ()
                {
                    Id = contentsPageId
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/" + contentsPageId);
    }}
