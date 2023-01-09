using System.Collections.Generic;
using System.Linq;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests
{

    public class CPDControllerTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private Mock<ILogger<CPDController>> _logger;
        private ContentfulCollection<PageViewModel> _pages;
        private ContentfulCollection<PageFooter> _footer;
        private ContentfulCollection<PageHeader> _header;
        private CPDController _target;

        [SetUp]
        public void Setup()
        {
            SetupModels();

            _contentfulClient = new Mock<IContentfulClient>(MockBehavior.Strict);
            _contentfulClient.Setup(c => c.GetEntries<PageViewModel>(It.IsAny<QueryBuilder<PageViewModel>>(), default)).ReturnsAsync(_pages);
            _contentfulClient.Setup(c => c.GetEntries<PageHeader>(new QueryBuilder<PageHeader>(), default)).ReturnsAsync(_header);
            _contentfulClient.Setup(c => c.GetEntries<PageFooter>(new QueryBuilder<PageFooter>(), default)).ReturnsAsync(_footer);
           _logger = new Mock<ILogger<CPDController>>();
           _target = new CPDController(_logger.Object, _contentfulClient.Object);
        }

        [Test]
        public void LandingPageReturnsModelOfTypePageViewModelTest()
        {
            var actual= _target.LandingPage(null, null, null, null);
            ViewResult viewResult = (ViewResult) actual.Result;
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
            Assert.AreEqual(model?.Items.First().PageType.PageType, pageType.ToString());
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
        }
    }
}