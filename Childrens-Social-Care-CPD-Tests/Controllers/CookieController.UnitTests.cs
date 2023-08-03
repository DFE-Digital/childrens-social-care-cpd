using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public partial class _CookieControllerTests
{
    private CookieController _cookieController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private ICpdContentfulClient _contentfulClient;
    private ILogger<CookieController> _logger;

    private void SetContent(Content content)
    {
        var contentCollection = new ContentfulCollection<Content>();

        contentCollection.Items = content == null
            ? new List<Content>()
            : contentCollection.Items = new List<Content> { content };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), default)
            .Returns(contentCollection);
    }

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<CookieController>>();
        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);

        var controllerContext = Substitute.For<ControllerContext>();
        controllerContext.HttpContext = _httpContext;

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(Arg.Is(typeof(IUrlHelperFactory))).Returns(Substitute.For<IUrlHelperFactory>());
        _httpContext.RequestServices.Returns(serviceProvider);

        _contentfulClient = Substitute.For<ICpdContentfulClient>();

        _cookieController = new CookieController(_logger, _contentfulClient);
        _cookieController.ControllerContext = controllerContext;
        _cookieController.TempData = Substitute.For<ITempDataDictionary>();
    }

    [Test]
    public async Task Cookies_Returns_404_When_No_Content_Found()
    {
        // arrange
        SetContent(null);

        // act
        var actual = await _cookieController.Cookies();

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Cookies_Sets_The_ViewState_ContextModel()
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
        await _cookieController.Cookies();
        var actual = _cookieController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(rootContent.Id);
        actual.Title.Should().Be(rootContent.Title);
        actual.Category.Should().Be(rootContent.Category);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Cookies_Sets_The_ContextModel_Preferences_Set_Value_Correctly(bool preferenceSet)
    {
        // arrange
        SetContent(new Content());

        // act
        await _cookieController.Cookies(preferenceSet: preferenceSet);
        var actual = _cookieController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.PreferenceSet.Should().Be(preferenceSet);
    }

    public static object[] SideMenuContent =
        {
            new object[] { new SideMenu() },
            new object[] { null },
        };

    [TestCaseSource(nameof(SideMenuContent))]
    public async Task Cookies_Sets_The_ContextModel_UseContainers_Ignoring_The_SideMenu_Value(SideMenu sideMenu)
    {
        // arrange
        var rootContent = new Content()
        {
            SideMenu = sideMenu
        };
        SetContent(rootContent);

        // act
        await _cookieController.Cookies();
        var actual = _cookieController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.UseContainers.Should().Be(true);
    }

    [Test]
    public async Task Cookies_Action_Should_Not_Show_Consent_Panel()
    {
        // arrange
        SetContent(new Content());

        // act
        await _cookieController.Cookies();
        var actual = _cookieController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.HideConsent.Should().Be(true);
    }
}
