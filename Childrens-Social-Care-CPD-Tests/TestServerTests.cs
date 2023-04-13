using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Azure;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests
{
    public class TestServerTests
    {
        [Test]
        public async Task Get_Request_Returns_Success_Test()
        {
            var requestUri = "/CPD/LandingPage";
            HttpClient client = GetClient();
            var actual = await client.GetAsync(requestUri);
            Assert.True(actual.IsSuccessStatusCode);
        }

        [Test]
        public async Task Get_Request_Returns_ErrorPage_With_Status_Code_When_PageNotFound_Test()
        {
            var requestUri = "/CPD/InvalidURL";
            HttpClient client = GetClient();
            var actual = await client.GetAsync(requestUri);
            
            Assert.AreEqual("/Error/Error/404", actual.RequestMessage.RequestUri.AbsolutePath);
        }

        [Test]
        public async Task SetCookie_Request_Post_Sets_Analytics_Cookie_Accept_Test()
        {
            var requestUri = "/Cookie/SetCookies";
            HttpClient client = GetClient();
            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("pageName", "HomePage"),
                new KeyValuePair<string, string>("pageType", "Master"),
                new KeyValuePair<string, string>("analyticsCookieConsent", "accept")
            });
            var actual = await client.PostAsync(requestUri, stringContent);
            
            Assert.True(actual.IsSuccessStatusCode);
            Assert.IsTrue(actual.Headers.GetValues("Set-Cookie").FirstOrDefault().Contains("cookie_consent=accept"));
        }

        [Test]
        public async Task SetCookie_Request_Post_Sets_Analytics_Cookie_Reject_Test()
        {
            var requestUri = "/Cookie/SetCookies";
            HttpClient client = GetClient();
            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("pageName", "HomePage"),
                new KeyValuePair<string, string>("pageType", "Master"),
                new KeyValuePair<string, string>("analyticsCookieConsent", "reject")
            });
            var actual = await client.PostAsync(requestUri, stringContent);

            Assert.True(actual.IsSuccessStatusCode);
            Assert.IsTrue(actual.Headers.GetValues("Set-Cookie").FirstOrDefault().Contains("cookie_consent=reject"));
        }

        private HttpClient GetClient()
        {
            var webApplicationFactory = new CPDTestServerApplication();
            HttpClient client = webApplicationFactory.CreateClient();
            return client;
        }
    }

     internal class CPDTestServerApplication : WebApplicationFactory<Program>
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        private ContentfulCollection<PageViewModel> _pages;
        public CPDTestServerApplication()
        {
            SetupModels();
            _contentfulDataService = new Mock<IContentfulDataService>();
            _contentfulDataService.Setup(c => c.GetViewData<PageViewModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_pages);
        }
       
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IContentfulDataService>((_) => _contentfulDataService.Object);
            });
            return base.CreateHost(builder);
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
        }
    }
}