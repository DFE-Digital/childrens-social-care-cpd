using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class CookieControllerServerTests
{
    private CpdTestServerApplication _application;
    private HttpClient _httpClient;
    private static string SetPrefencesUrl = "/cookies/setpreferences";

    [SetUp]
    public void SetUp()
    {
        _application = new CpdTestServerApplication();
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
        _application.CpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);

        _httpClient = _application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region SetPreferences

    [Test]
    [Ignore("")]
    [TestCase(CookieHelper.ANALYTICSCOOKIEACCEPTED)]
    [TestCase(CookieHelper.ANALYTICSCOOKIEREJECTED)]
    public async Task SetPreferences_Sets_Cookie(string consentValue)
    {
        // arrange
        var formContent = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("consentValue", consentValue),
        });

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        var cookie = response.Headers.First(x => x.Key == "Set-Cookie");
        cookie.Value.First().Should().StartWith($"cookie_consent={consentValue};");
    }

    [Test]
    [Ignore("")]
    [TestCase("")]
    [TestCase("invalid")]
    public async Task SetPreferences_Clear_Cookie(string consentValue)
    {
        // arrange
        var formContent = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("consentValue", consentValue),
        });

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        var cookie = response.Headers.FirstOrDefault(x => x.Key == "Set-Cookie");
        cookie.Value.First().Should().StartWith($"cookie_consent=;");
    }

    #endregion
}
