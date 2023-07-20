using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;
using System;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public partial class _CookieControllerTests
{
    private CookieController _cookieController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private IContentfulDataService _contentfulDataService;
    private ICpdContentfulClient _contentfulClient;
    private ILogger<CookieController> _logger;

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
        _contentfulDataService = Substitute.For<IContentfulDataService>();

        _cookieController = new CookieController(_logger, _contentfulDataService, _contentfulClient);
        _cookieController.ControllerContext = controllerContext;
        _cookieController.TempData = Substitute.For<ITempDataDictionary>();
    }

    [Test]
    public async Task Cookies_Returns_404_When_No_Content_Found()
    {
        // arrange
        var noContent = new ContentfulCollection<Content>() { Items = new List<Content>() };
        _cookies[SiteConstants.ANALYTICSCOOKIENAME].Returns(SiteConstants.ANALYTICSCOOKIEACCEPTED);
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(noContent);

        // act
        var actual = await _cookieController.Cookies();

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Cookies_Sets_The_ViewState_PageName()
    {
        // arrange
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
        _cookies[SiteConstants.ANALYTICSCOOKIENAME].Returns(SiteConstants.ANALYTICSCOOKIEACCEPTED);
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

        // act
        var actual = await _cookieController.Cookies();

        // assert
        _cookieController.ViewData["PageName"].Should().Be("cookies");
    }

    [Test]
    public async Task Cookies_Sets_The_ViewState_Title()
    {
        // arrange
        var pageTitle = "A title";
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() { Title = pageTitle } } };
        _cookies[SiteConstants.ANALYTICSCOOKIENAME].Returns(SiteConstants.ANALYTICSCOOKIEACCEPTED);
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

        // act
        var actual = await _cookieController.Cookies();

        // assert
        _cookieController.ViewData["Title"].Should().Be(pageTitle);
    }

    [Test]
    public async Task Cookies_Sets_The_ViewState_ContentStack()
    {
        // arrange
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
        _cookies[SiteConstants.ANALYTICSCOOKIENAME].Returns(SiteConstants.ANALYTICSCOOKIEACCEPTED);
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

        // act
        var actual = await _cookieController.Cookies();

        // assert
        _cookieController.ViewData["ContentStack"].Should().BeOfType<Stack<string>>();
    }
}
