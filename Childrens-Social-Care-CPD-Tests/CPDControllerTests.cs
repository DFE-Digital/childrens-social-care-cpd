using System.Collections.Generic;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Childrens_Social_Care_CPD_Tests
{
    public class CPDControllerTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private Mock<ILogger<CPDController>> _logger;
        private ContentfulCollection<PageViewModel> _pages;
        private ContentfulCollection<PageFooter> _footer;
        private ContentfulCollection<PageHeader> _header;

        [SetUp]
        public void Setup()
        {
            SetupModels();

            _contentfulClient = new Mock<IContentfulClient>();
            _contentfulClient.Setup(c => c.GetEntries<PageViewModel>(new QueryBuilder<PageViewModel>(), default)).ReturnsAsync(_pages);
            _contentfulClient.Setup(c => c.GetEntries<PageHeader>(new QueryBuilder<PageHeader>(), default)).ReturnsAsync(_header);
            _contentfulClient.Setup(c => c.GetEntries<PageFooter>(new QueryBuilder<PageFooter>(), default)).ReturnsAsync(_footer);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
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
                        Paragraphs = null,
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