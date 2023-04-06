using System.Collections.Generic;
using System.Linq;
using Childrens_Social_Care_CPD.ActionFilters;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ActionDescriptor = Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor;
using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace Childrens_Social_Care_CPD_Tests
{

    public class ActionFilterTests
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        private PageFooter _footer;
        private PageHeader _header;
        private ContentfulCollection<CookieBanner> _banner;
        private CPDActionFilter _target;
        private CPDController _controller;
        private Mock<ILogger<CPDActionFilter>> _logger;

        [SetUp]
        public void Setup()
        {
            SetupModels();
            _contentfulDataService = new Mock<IContentfulDataService>(MockBehavior.Loose);
            _contentfulDataService.Setup(c => c.GetFooterData()).ReturnsAsync(_footer);
            _contentfulDataService.Setup(c => c.GetHeaderData()).ReturnsAsync(_header);
            _logger = new Mock<ILogger<CPDActionFilter>>();
            _target = new CPDActionFilter(_logger.Object, _contentfulDataService.Object);
            _controller =  new CPDController(null,null);
        }

        [Test]
        public void ActionFilterSetsPageHeaderTest()
        {
            var context = SetActionExecutingContext();
            _target.OnActionExecuting(context);
            var actual = _controller.ViewData["PageHeader"] as PageHeader;
            Assert.IsNotNull(actual);
            Assert.AreEqual("TestPageHeader", actual.Header);
        }

        [Test]
        public void ActionFilterSetsPageFooterTest()
        {
            var context = SetActionExecutingContext();
            _target.OnActionExecuting(context);
            var actual = _controller.ViewData["PageFooter"] as PageFooter;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.FooterLinks.Count);
            Assert.AreEqual("TestLink", actual.FooterLinks.First().LinkText);
        }



        private ActionExecutingContext SetActionExecutingContext()
        {
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext();
            var context = new Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                _controller);
            return context;
        }

        private void SetupModels()
        {
            _header = new PageHeader
            {
                Header = "TestPageHeader",
                PrototypeHeader = "TestHeader",
                PrototypeText = new Document(),
                PrototypeTextHtml = "TestHtml",
            };

            _footer = new PageFooter()
            {
                        FooterLinks = new()
                        {
                            new Link
                            {
                                LinkText = "TestLink",
                                LinkURL = "TestUrl",
                                LinkSection = null,
                                PageType = null,
                                RedirectPageName = null,
                                SideNaveGroupText = null,
                                SortOrder = 1
                            }
                        },
                        LicenceDescription = new Document(),
                        CopyrightLink = new Link(),
                        LicenceDescriptionText = "TestDescription"
            };

            _banner = new ContentfulCollection<CookieBanner>()
            {
                Items = new List<CookieBanner>()
                {
                   new CookieBanner ()
                   {
                    AcceptCookieButtonText = "AcceptAnalytics",
                    AcceptedCookieMessage = new Document(),
                    CookieBannerHeading = "TestCookie",
                    ViewCookiesLink = new Link(),
                    RejectedCookieMessage = new Document(),
                    HideCookieMessageButtonText = "HideMessage"
                   }
                }
            };
        }
    }
}