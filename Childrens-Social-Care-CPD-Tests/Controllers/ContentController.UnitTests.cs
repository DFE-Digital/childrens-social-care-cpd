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
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers
{
    public class ContentControllerTests
    {
        private ContentController _contentController;
        private Mock<IRequestCookieCollection> _cookies;
        private Mock<HttpContext> _httpContext;
        private Mock<HttpRequest> _httpRequest;
        private Mock<IContentfulDataService> _contentfulDataService;
        private Mock<ICpdContentfulClient> _contentfulClient;

        [SetUp]
        public void SetUp()
        {
            _cookies = new Mock<IRequestCookieCollection>();
            _httpContext = new Mock<HttpContext>();
            _httpRequest = new Mock<HttpRequest>();
            var controllerContext = new Mock<ControllerContext>();

            _httpRequest.Setup(x => x.Cookies).Returns(_cookies.Object);
            _httpContext.Setup(x => x.Request).Returns(_httpRequest.Object);
            controllerContext.Object.HttpContext = _httpContext.Object;

            _contentfulClient = new Mock<ICpdContentfulClient>();
            _contentfulDataService = new Mock<IContentfulDataService>();

            _contentController = new ContentController(_contentfulClient.Object, _contentfulDataService.Object);
            _contentController.ControllerContext = controllerContext.Object;
        }

        [Test]
        public async Task Cookie_Content_Is_Fetched_When_No_Analytics_Cookie_Exists()
        {
            // arrange
            var cookieBanner = new CookieBanner();
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(false);
            _contentfulDataService.Setup(x => x.GetCookieBannerData()).Returns(Task.FromResult(cookieBanner));

            // act
            await _contentController.Index("home");

            // assert
            _contentController.ViewData.ContainsKey("CookieBanner").Should().BeTrue();
            _contentController.ViewData["CookieBanner"].Should().Be(cookieBanner);
        }

        [Test]
        public async Task Cookie_Content_Is_Not_Fetched_When_Analytics_Cookie_Exists()
        {
            // arrange
            var cookieBanner = new CookieBanner();
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulDataService.Setup(x => x.GetCookieBannerData()).Returns(Task.FromResult(cookieBanner));

            // act
            await _contentController.Index("home");

            // assert
            _contentController.ViewData.ContainsKey("CookieBanner").Should().BeFalse();
        }

        [Test]
        public async Task Index_Returns_404_When_No_Content_Found()
        {
            // arrange
            var noContent = new ContentfulCollection<Content>() { Items = new List<Content>() };
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Content>>(), default)).ReturnsAsync(noContent);

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
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Content>>(), default)).ReturnsAsync(contentCollection);

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
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Content>>(), default)).ReturnsAsync(contentCollection);

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
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Content>>(), default)).ReturnsAsync(contentCollection);

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
            _cookies.Setup(x => x.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME)).Returns(true);
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Content>>(), default)).ReturnsAsync(contentCollection);

            // act
            var actual = await _contentController.Index(pageName);

            // assert
            _contentController.ViewData["ContentStack"].Should().BeOfType<Stack<string>>();
        }
    }
}
