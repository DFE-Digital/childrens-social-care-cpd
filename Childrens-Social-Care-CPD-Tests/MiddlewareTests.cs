using System.Net;
using System.Threading.Tasks;
using Childrens_Social_Care_CPD;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;


namespace Childrens_Social_Care_CPD_Tests
{

    public class MiddlewareTests
    {
        [Test]
        public async Task CheckRequestHeader_ReturnsBadRequestForInvalidHeader()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseMiddleware<CheckRequestHeaderMiddleware>();
                        });
                })
                .StartAsync();

            var response = await host.GetTestClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}