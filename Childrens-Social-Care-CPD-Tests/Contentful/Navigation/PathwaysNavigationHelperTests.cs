using System.Collections.Generic;
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
    }

    [Test]
    public async Task Page_Of_Type_Pathways_Contents_Page_Should_Have_First_Page_Of_First_Section_Id_As_Next_Url () {

        // setup
        var trainingPageId = "TRAINING_PAGE_ID";
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = trainingPageId
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/" + trainingPageId);
    }

    [Test]
    public async Task Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_First_Section_Has_No_Pages () {

        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection ()
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public async Task Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_Pathway_Has_No_Sections () {

        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule ()
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public async Task Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_Page_Has_No_Pathway_Modules () {

        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }
}