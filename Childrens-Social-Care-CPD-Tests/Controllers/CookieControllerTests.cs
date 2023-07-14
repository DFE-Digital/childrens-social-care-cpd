using System.Collections.Generic;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ActionDescriptor = Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor;
using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;


namespace Childrens_Social_Care_CPD_Tests.Controllers
{

    public class CookieControllerTests
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        private Mock<ILogger<CookieController>> _logger;
        private ContentfulCollection<PageViewModel> _pages;
        private ContentfulCollection<CookieBanner> _banner;
        private CookieController _target;

        [SetUp]
        public void Setup()
        {
            SetupModels();
            _contentfulDataService = new Mock<IContentfulDataService>(MockBehavior.Strict);
            _contentfulDataService.Setup(c => c.GetViewData<PageViewModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_pages);
            _logger = new Mock<ILogger<CookieController>>();
            _target = new CookieController(_logger.Object, _contentfulDataService.Object);
        }

        [Test]
        public void SetCookiesRedirectToGetCookiesTest()
        {
            var actual = _target.SetCookies(null, null, null, null, null, null);
            RedirectToActionResult viewResult = (RedirectToActionResult)actual;
            Assert.AreEqual("LandingPage", viewResult.ActionName);
        }

        private void SetupModels()
        {
            _pages = new ContentfulCollection<PageViewModel>
            {
                Items = new List<PageViewModel>
                {
                    new PageViewModel
                    {
                        PageName = new ContentPageName { PageName = "TestPage" },
                        PageTitle = "Test Title",
                        PageHeading = "TestHeading",
                        PageSubHeading = "TestSubHeading",
                        PageType = new ContentPageType { PageType = "Master" },
                        Cards = new List<Card>
                        {
                            new Card
                            {
                                CardHeader = "TestCardHeading",
                                CardContents = "Test contents",
                                CardDescription = "Test Description",
                                SortOrder = 0,
                                PageType = null,
                                RedirectPageName = null
                            }
                        },
                        Links = new List<Link>
                        {
                            new Link
                            {
                                LinkText = "TestLink",
                                LinkURL = "#",
                                SortOrder = 0
                            }
                        },
                        Labels = new List<Label>
                        {
                            new Label
                            {
                                labelHeading = "TestLabelHeading",
                                LabelText = "TestLabelText",
                                LabelType = null,
                                LabelTextLink = "#",
                                SortOrder = 0
                            }
                        },
                        RichTexts = new List<RichText>
                        {
                            new RichText
                            {
                                Heading = "TestHeading",
                                SubHeading = "TestSubHeading",
                                RichTextContents = new Document(),
                                SortOrder = 0
                            }
                        }
                    }
                }
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