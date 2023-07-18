using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Interfaces;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

internal class CpdTestServerApplication : WebApplicationFactory<Program>
{
    private IContentfulDataService _contentfulDataService;
    private ICpdContentfulClient _cpdContentfulClient;

    public CpdTestServerApplication()
    {
        _contentfulDataService = Substitute.For<IContentfulDataService>();
        _cpdContentfulClient = Substitute.For<ICpdContentfulClient>();

        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
        _cpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient((_) => _contentfulDataService);
            services.AddTransient((_) => _cpdContentfulClient);
        });
        return base.CreateHost(builder);
    }
}

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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);
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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);
        var cookie = response.Headers.FirstOrDefault(x => x.Key == "Set-Cookie");
        cookie.Value.First().Should().StartWith($"cookie_consent=;");
    }

    [Test]
    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content?prefsset=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content?prefsset=true")]
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
        response.Headers.Location.PathAndQuery.Should().EndWith(expected);
    }

    [Test]
    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, "/content/test", null, "/content/test?prefsset=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, "/content/test", null, "/content/test?prefsset=true")]
    [TestCase(null, "/content/test", null, "/content/test")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, null, "/content/test", "/content/test?prefsset=true")]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, null, "/content/test", "/content/test?prefsset=true")]
    [TestCase(null, null, "/content/test", "/content/test")]
    public async Task SetPreferences_Redirects_To_Correct_Url_When_Host_Matches(string consentValue, string referer, string redirectTo, string expected)
    {
        // arrange
        var content = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("consentValue", consentValue),
        };

        if (!string.IsNullOrEmpty(redirectTo))
        {
            var uri = new Uri(_httpClient.BaseAddress, redirectTo);
            content.Add(new KeyValuePair<string, string>("redirectTo", uri.ToString()));
        }

        var formContent = new FormUrlEncodedContent(content);

        if (!string.IsNullOrEmpty(referer))
        {
            var uri = new Uri(_httpClient.BaseAddress, referer);
            _httpClient.DefaultRequestHeaders.Referrer = uri;
        }

        // act
        var response = await _httpClient.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.PathAndQuery.Should().EndWith(expected);
    }

    #endregion

    #region Cookies

    [Test]
    public async Task Cookies_Returns_Page()
    {
        // act
        var response = await _httpClient.GetAsync(CookiesUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
