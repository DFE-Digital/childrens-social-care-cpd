using System.Collections.Generic;
using System.Linq;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ActionDescriptor = Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor;
using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;


namespace Childrens_Social_Care_CPD_Tests
{

    public class CPDControllerTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private Mock<ILogger<CPDController>> _logger;
        private ContentfulCollection<PageViewModel> _pages;
        private ContentfulCollection<PageFooter> _footer;
        private ContentfulCollection<PageHeader> _header;
        private ContentfulCollection<CookieBanner> _banner;
        private CPDController _target;

        [SetUp]
        public void Setup()
        {
            SetupModels();


            _contentfulClient = new Mock<IContentfulClient>(MockBehavior.Strict);
            _contentfulClient.Setup(c => c.GetEntries<PageViewModel>(It.IsAny<QueryBuilder<PageViewModel>>(), default)).ReturnsAsync(_pages);
            _contentfulClient.Setup(c => c.GetEntries<PageHeader>(It.IsAny<QueryBuilder<PageHeader>>(), default)).ReturnsAsync(_header);
            _contentfulClient.Setup(c => c.GetEntries<PageFooter>(It.IsAny<QueryBuilder<PageFooter>>(), default)).ReturnsAsync(_footer);
            _contentfulClient.Setup(c => c.GetEntries<CookieBanner>(It.IsAny<QueryBuilder<CookieBanner>>(), default)).ReturnsAsync(_banner);
            _logger = new Mock<ILogger<CPDController>>();
            _target = new CPDController(_logger.Object, _contentfulClient.Object);
        }

        [Test]
        public void LandingPageReturnsModelOfTypePageViewModelTest()
        {
            var actual = _target.LandingPage(null, null, null, null);
            ViewResult viewResult = (ViewResult)actual.Result;
            Assert.IsInstanceOf<ContentfulCollection<PageViewModel>>(viewResult.Model);
        }

        [Test]
        [TestCase(PageTypes.Master)]
        [TestCase(PageTypes.Cards)]
        [TestCase(PageTypes.PathwayDetails)]
        [TestCase(PageTypes.Programmes)]
        public void LandingPageReturnsCorrectPageTemplateTest(PageTypes pageType)
        {
            var actual = _target.LandingPage(null, pageType.ToString(), null, null);
            ViewResult viewResult = (ViewResult)actual.Result;
            var model = viewResult.ViewData.Model as ContentfulCollection<PageViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Items.First().PageType.PageType, pageType.ToString());
        }

        [Test]
        public void AppInfoReturnsApplicationInfoTest()
        {
            var actual = _target.AppInfo();
            var c = actual.Value;
            Assert.IsInstanceOf<JsonResult>(actual);
            Assert.IsNotNull(((ApplicationInfo)actual.Value).Environment);
        }

        [Test]
        public void ActionFilterSetsPageHeaderTest()
        {
            var context = SetActionExecutingContext();
            _target.OnActionExecuting(context);
            var actual = _target.ViewData["PageHeader"] as PageHeader;
            Assert.IsNotNull(actual);
            Assert.AreEqual("TestPageHeader", actual.Header);
        }

        [Test]
        public void ActionFilterSetsPageFooterTest()
        {
            var context = SetActionExecutingContext();
            _target.OnActionExecuting(context);
            var actual = _target.ViewData["PageFooter"] as PageFooter;
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
                _target);
            return context;
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

            _header = new ContentfulCollection<PageHeader>()
            {
                Items = new List<PageHeader>()
                {
                    new PageHeader()
                    {
                        Header = "TestPageHeader",
                        PrototypeHeader = "TestHeader",
                        PrototypeText =  new Document(),
                        PrototypeTextHtml = "TestHtml",
                    }
                }
            };

            _footer = new ContentfulCollection<PageFooter>
            {
                Items = new List<PageFooter>
                {
                    new PageFooter
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
                    }
                },
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