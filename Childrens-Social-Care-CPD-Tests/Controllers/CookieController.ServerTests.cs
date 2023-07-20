using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using NUnit.Framework;
using System;
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
    private static string SetPrefencesUrl = "/Cookie/SetPreferences";
    private static string CookiesUrl = "/cookies";

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
    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED)]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED)]
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

    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content?preferenceSet=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content?preferenceSet=true")]
    [TestCase(null, "/content")]
    public async Task SetPreferences_Redirects_To_Correct_Url_When_No_Referer_Or_Redirect(string consentValue, string expected)
    {
        // arrange
        var formContent = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("consentValue", consentValue),
        });

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.OriginalString.Should().EndWith(expected);
    }

    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content/item?preferenceSet=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content/item?preferenceSet=true")]
    [TestCase(null, "/content/item")]
    public async Task SetPreferences_Redirect_Falls_Back_To_Referrer(string consentValue, string expected)
    {
        // arrange
        var referer = "/content/item";
        var uri = new Uri(_httpClient.BaseAddress, referer);
        _httpClient.DefaultRequestHeaders.Referrer = uri;

        var formContent = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("consentValue", consentValue),
        });

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.PathAndQuery.Should().Be(expected);
    }

    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content/item?preferenceSet=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content/item?preferenceSet=true")]
    [TestCase(null, "/content/item")]
    public async Task SetPreferences_Redirects_Local_Relative_Url(string consentValue, string expected)
    {
        // arrange
        var redirectTo = "/content/item";
        var content = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("consentValue", consentValue),
            new KeyValuePair<string, string>("redirectTo", redirectTo)
        };

        var formContent = new FormUrlEncodedContent(content);

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.OriginalString.Should().Be(expected);
    }

    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content/item?preferenceSet=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content/item?preferenceSet=true")]
    [TestCase(null, "/content/item")]
    public async Task SetPreferences_Redirects_Local_Absolute_Url(string consentValue, string expected)
    {
        // arrange
        var redirectTo = "/content/item";
        var uri = new Uri(_httpClient.BaseAddress, redirectTo);
        var expectedUri = new Uri(_httpClient.BaseAddress, expected);
        var content = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("consentValue", consentValue),
            new KeyValuePair<string, string>("redirectTo", uri.ToString())
        };

        var formContent = new FormUrlEncodedContent(content);

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.OriginalString.Should().Be(expectedUri.ToString());
    }

    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content?preferenceSet=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content?preferenceSet=true")]
    [TestCase(null, "/content")]
    public async Task SetPreferences_Redirects_Non_Local_Absolute_Url_To_Local(string consentValue, string expected)
    {
        // arrange
        var redirectTo = "http://www.google.com";
        var uri = new Uri(redirectTo);
        var content = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("consentValue", consentValue),
            new KeyValuePair<string, string>("redirectTo", uri.ToString())
        };

        var formContent = new FormUrlEncodedContent(content);

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.OriginalString.Should().Be(expected);
    }

    #endregion

    #region Cookies

    [Test]
    public async Task Cookies_Returns_Page()
    {
        // act
        var response = await _httpClient.GetAsync(CookiesUrl);

        var content = await response.Content.ReadAsStringAsync();
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
