using System;
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
using Microsoft.Rest;
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
        public async Task Get_Request_Returns_ErrorPage_When_Internal_Server_Error_Test()
        {
            var requestUri = "/CPD/LandingPage";
            HttpClient client = GetClient(testExceptionScenario:true);
            var actual = await client.GetAsync(requestUri);

            Assert.AreEqual(StatusCodes.Status500InternalServerError, (int)actual.StatusCode) ;
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

        private HttpClient GetClient(bool testExceptionScenario = false)
        {
            var webApplicationFactory = new CPDTestServerApplication(testExceptionScenario);
            HttpClient client = webApplicationFactory.CreateClient();
            return client;
        }
    }

     internal class CPDTestServerApplication : WebApplicationFactory<Program>
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        public CPDTestServerApplication(bool testExceptionScenario)
        {
            _contentfulDataService = new Mock<IContentfulDataService>();
            if(testExceptionScenario)
            {
                _contentfulDataService.Setup(c => c.GetViewData<PageViewModel>(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());
            }
            else
            {
                _contentfulDataService.Setup(c => c.GetViewData<PageViewModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(MockData.Pages);
            }
        }
       
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IContentfulDataService>((_) => _contentfulDataService.Object);
            });
            return base.CreateHost(builder);
        }
    }
}