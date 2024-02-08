using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ContentControllerTests
{
    private ContentController _contentController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private IContentRepository _contentRepository;
    private IMemoryCache _cache;

#pragma warning disable S1121 // Assignments should not be made from within sub-expressions

    private void SetContent(Content content)
    {
        var contentCollection = new ContentfulCollection<Content>();

        contentCollection.Items = content == null
            ? new List<Content>()
            : contentCollection.Items = new List<Content> { content };

        _contentRepository.FetchPageContentAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(content);
    }

#pragma warning restore S1121 // Assignments should not be made from within sub-expressions

    [SetUp]
    public void SetUp()
    {
        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();
        var controllerContext = Substitute.For<ControllerContext>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);
        controllerContext.HttpContext = _httpContext;

        _contentRepository = Substitute.For<IContentRepository>();
        _cache = Substitute.For<IMemoryCache>();

        _contentController = new ContentController(_contentRepository, _cache)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [Test]
    public async Task Index_Returns_404_When_No_Content_Found()
    {
        // arrange
        SetContent(null);

        // act
        var actual = await _contentController.Index("home");

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Index_Returns_View()
    {
        // arrange
        SetContent(new Content());

        // act
        var actual = await _contentController.Index("home");

        // assert
        actual.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task Index_Sets_The_ViewState_ContextModel()
    {
        // arrange
        var rootContent = new Content()
        {
            Id = "a/value",
            Category = "A Category",
            Title = "A Title",
        };
        SetContent(rootContent);

        // act
        await _contentController.Index("home");
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(rootContent.Id);
        actual.Title.Should().Be(rootContent.Title);
        actual.Category.Should().Be(rootContent.Category);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Index_Sets_The_ContextModel_Preferences_Set_Value_Correctly(bool preferenceSet)
    {
        // arrange
        SetContent(new Content());

        // act
        await _contentController.Index("home", preferenceSet);
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.PreferenceSet.Should().Be(preferenceSet);
    }

    private static readonly object[] _navigationMenuContent =
    {
        new object[] { new NavigationMenu() },
        new object[] { null },
    };

    [TestCaseSource(nameof(_navigationMenuContent))]
    public async Task Index_Sets_The_ContextModel_UseContainers_From_SideMenu_Value_Correctly(NavigationMenu navigationMenu)
    {
        // arrange
        var rootContent = new Content()
        {
            Navigation = navigationMenu
        };
        var expected = navigationMenu == null;
        SetContent(rootContent);

        // act
        await _contentController.Index("home");
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.UseContainers.Should().Be(expected);
    }
}
