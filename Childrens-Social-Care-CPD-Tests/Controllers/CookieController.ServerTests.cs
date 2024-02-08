using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class CookieControllerServerTests
{
    private CpdTestServerApplication _application;
    private static string SetPrefencesUrl = "/cookies/setpreferences";

    [SetUp]
    public void SetUp()
    {
        _application = new CpdTestServerApplication();
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() { new Content() } };
        _application.CpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), default).Returns(contentCollection);
    }

    #region SetPreferences

    [Test]
    [TestCase(CookieHelper.ANALYTICSCOOKIEACCEPTED)]
    [TestCase(CookieHelper.ANALYTICSCOOKIEREJECTED)]
    public async Task SetPreferences_Sets_Cookie(string consentValue)
    {
        // arrange
        var antiforgeryTokens = await _application.GetAntiforgeryTokensAsync();
        var cookies = new CookieContainerHandler();
        cookies.Container.Add(_application.Server.BaseAddress, new Cookie(antiforgeryTokens.CookieName, antiforgeryTokens.CookieValue));
        var client = _application.CreateDefaultClient(cookies);

        var formContent = new FormUrlEncodedContent(new Dictionary<string, string> { ["consentValue"] = consentValue });
        formContent.Headers.Add(antiforgeryTokens.HeaderName, antiforgeryTokens.RequestToken);

        // act
        var response = await client.PostAsync(SetPrefencesUrl, formContent);

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
        var antiforgeryTokens = await _application.GetAntiforgeryTokensAsync();
        var cookies = new CookieContainerHandler();
        cookies.Container.Add(_application.Server.BaseAddress, new Cookie(antiforgeryTokens.CookieName, antiforgeryTokens.CookieValue));
        var client = _application.CreateDefaultClient(cookies);

        var formContent = new FormUrlEncodedContent(new Dictionary<string, string> { ["consentValue"] = consentValue });
        formContent.Headers.Add(antiforgeryTokens.HeaderName, antiforgeryTokens.RequestToken);

        // act
        var response = await client.PostAsync(SetPrefencesUrl, formContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        var cookie = response.Headers.FirstOrDefault(x => x.Key == "Set-Cookie");
        cookie.Value.First().Should().StartWith($"cookie_consent=;");
    }

    #endregion
}
