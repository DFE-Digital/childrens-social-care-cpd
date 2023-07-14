using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers
{
    public class ContentControllerTests
    {
        private ContentController _contentController;
        private IRequestCookieCollection _cookies;
        private HttpContext _httpContext;
        private HttpRequest _httpRequest;
        private IContentfulDataService _contentfulDataService;
        private ICpdContentfulClient _contentfulClient;

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

            _contentfulClient = Substitute.For<ICpdContentfulClient>();
            _contentfulDataService = Substitute.For<IContentfulDataService>();

            _contentController = new ContentController(_contentfulClient, _contentfulDataService);
            _contentController.ControllerContext = controllerContext;
            _contentController.TempData = Substitute.For<ITempDataDictionary>();
        }

        [Test]
        public async Task Cookie_Content_Is_Fetched_When_No_Analytics_Cookie_Exists()
        {
            // arrange
            var cookieBanner = new CookieBanner();
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(false);
            _contentfulDataService.GetCookieBannerData().Returns(Task.FromResult(cookieBanner));

            // act
            await _contentController.Index("home");

            // assert
            _contentController.ViewData["CookieBanner"].Should().Be(cookieBanner);
        }

        [Test]
        public async Task Cookie_Content_Is_Not_Fetched_When_Analytics_Cookie_Exists()
        {
            // arrange
            var cookieBanner = new CookieBanner();
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulDataService.GetCookieBannerData().Returns(Task.FromResult(cookieBanner));

            // act
            await _contentController.Index("home");

            // assert
            _contentController.ViewData["CookieBanner"].Should().BeNull();
        }

        [Test]
        public async Task Index_Returns_404_When_No_Content_Found()
        {
            // arrange
            var noContent = new ContentfulCollection<Content>() { Items = new List<Content>() };
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(noContent);

            // act
            var actual = await _contentController.Index("home");

            // assert
            actual.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Index_Returns_View()
        {
            // arrange
            var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

            // act
            var actual = await _contentController.Index("home");

            // assert
            actual.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task Index_Sets_The_ViewState_PageName()
        {
            // arrange
            var pageName = "home";
            var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

            // act
            var actual = await _contentController.Index(pageName);

            // assert
            _contentController.ViewData["PageName"].Should().Be(pageName);
        }

        [Test]
        public async Task Index_Sets_The_ViewState_Title()
        {
            // arrange
            var pageName = "home";
            var pageTitle = "A title";
            var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() { Title = pageTitle } } };
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

            // act
            var actual = await _contentController.Index(pageName);

            // assert
            _contentController.ViewData["Title"].Should().Be(pageTitle);
        }

        [Test]
        public async Task Index_Sets_The_ViewState_ContentStack()
        {
            // arrange
            var pageName = "home";
            var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
            _cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME).Returns(true);
            _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

            // act
            var actual = await _contentController.Index(pageName);

            // assert
            _contentController.ViewData["ContentStack"].Should().BeOfType<Stack<string>>();
        }
    }
}
