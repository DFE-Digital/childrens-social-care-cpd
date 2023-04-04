using System.Collections.Generic;
using System.Linq;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Services;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Moq;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests
{

    public class ContentfulDataServiceTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private ContentfulCollection<PageViewModel> _pages;
        private ContentfulCollection<PageFooter> _footer;
        private ContentfulCollection<PageHeader> _header;
        private ContentfulCollection<CookieBanner> _banner;
        private IContentfulDataService _target;

        [SetUp]
        public void Setup()
        {
            SetupModels();


            _contentfulClient = new Mock<IContentfulClient>(MockBehavior.Strict);
            _contentfulClient.Setup(c => c.GetEntries<PageViewModel>(It.IsAny<QueryBuilder<PageViewModel>>(), default)).ReturnsAsync(_pages);
            _contentfulClient.Setup(c => c.GetEntries<PageHeader>(It.IsAny<QueryBuilder<PageHeader>>(), default)).ReturnsAsync(_header);
            _contentfulClient.Setup(c => c.GetEntries<PageFooter>(It.IsAny<QueryBuilder<PageFooter>>(), default)).ReturnsAsync(_footer);
            _contentfulClient.Setup(c => c.GetEntries<CookieBanner>(It.IsAny<QueryBuilder<CookieBanner>>(), default)).ReturnsAsync(_banner);

            _target = new ContentfulDataService(_contentfulClient.Object);
        }

        [Test]
        public void GetViewDataReturnsDataWithPageViewModelTest()
        {
            var actual = _target.GetViewData<PageViewModel>("TestPage", "Master").Result;
            Assert.IsInstanceOf<ContentfulCollection<PageViewModel>>(actual);
            Assert.AreEqual("Test Description", actual.FirstOrDefault().Cards.FirstOrDefault().CardDescription);
        }
        [Test]
        public void GetViewDataReturnsDataForMasterPageWhenNoPageNameOrPageTypeProvidedTest()
        {
            var actual = _target.GetViewData<PageViewModel>(null, null).Result;
            Assert.IsInstanceOf<ContentfulCollection<PageViewModel>>(actual);
            Assert.AreEqual(PageTypes.Master.ToString(), actual.FirstOrDefault().PageType.PageType.ToString());
        }

        [Test]
        public void GetHeaderDataReturnsDataWithPageHeaderModelTest()
        {
            var actual = _target.GetHeaderData().Result;
            Assert.IsInstanceOf<PageHeader>(actual);
            Assert.AreEqual("TestPageHeader", actual.Header);
        }

        [Test]
        public void GetFooterDataReturnsDataWithPageFooterModelTest()
        {
            var actual = _target.GetFooterData().Result;
            Assert.IsInstanceOf<PageFooter>(actual);
            Assert.AreEqual(1, actual.FooterLinks.Count);
        }

        [Test]
        public void GetCookieDataReturnsDataWithCookieBannerModelTest()
        {
            var actual = _target.GetCookieBannerData().Result;
            Assert.IsInstanceOf<CookieBanner>(actual);
            Assert.AreEqual("AcceptAnalytics", actual.AcceptCookieButtonText);
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