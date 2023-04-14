using System.Collections.Generic;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD_Tests
{
    public static class MockData
    {
        private static readonly ContentfulCollection<PageViewModel> _pages;
        private static readonly PageFooter _footer;
        private static readonly PageHeader _header;
        private static readonly ContentfulCollection<CookieBanner> _banner;
        
        static MockData()
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

        public static ContentfulCollection<PageViewModel> Pages
        {
            get
            {
                return _pages;
            }
        }

        public static PageHeader Header
        {
            get
            {
                return _header;
            }
        }

        public static PageFooter Footer
        {
            get
            {
                return _footer;
            }
        }

        public static ContentfulCollection<CookieBanner> Banner
        {
            get
            {
                return _banner;
            }
        }

    }
}