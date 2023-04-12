using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure;
using Childrens_Social_Care_CPD.Interfaces;
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
        public async Task WebPage_Get_Request_Returns_Success_Test()
        {
            var webApplicationFactory = new CPDTestServerApplication();
            HttpClient client = webApplicationFactory.CreateClient();
            var actual = await client.GetAsync("/CPD/LandingPage");
            Assert.True(actual.IsSuccessStatusCode);
        }

        [Test]
        public async Task WebPage_Get_Request_Returns_ErrorPage_With_Status_Code_When_PageNotFound_Test()
        {
            var webApplicationFactory = new CPDTestServerApplication();
            HttpClient client = webApplicationFactory.CreateClient();
            var actual = await client.GetAsync("/CPD/InvalidURL");
            Assert.AreEqual("/Error/Error/404", actual.RequestMessage.RequestUri.AbsolutePath);
        }
    }

     internal class CPDTestServerApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IContentfulDataService>((_) => new Mock<IContentfulDataService>().Object);
            });
            return base.CreateHost(builder);
        }
    }
}