using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests;

public class CookieHelperTests
{
    private ICookieHelper _cookieHelper;
    private IApplicationConfiguration _applicationConfiguration;

    [SetUp]
    public void Setup()
    {
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _cookieHelper = new CookieHelper(_applicationConfiguration);
    }

    [TestCase(AnalyticsConsentState.Accepted, CookieHelper.ANALYTICSCOOKIEACCEPTED)]
    [TestCase(AnalyticsConsentState.Rejected, CookieHelper.ANALYTICSCOOKIEREJECTED)]
    public void SetResponseAnalyticsCookieState_Sets_Correct_Value(AnalyticsConsentState state, string expectedValue)
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();

        // act
        _cookieHelper.SetResponseAnalyticsCookieState(httpContext, state);

        // assert
        httpContext.Response.Cookies.Received().Append(CookieHelper.ANALYTICSCOOKIENAME, expectedValue, Arg.Any<CookieOptions>());
    }

    [Test]
    public void SetResponseAnalyticsCookieState_Clears_Cookie()
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();

        // act
        _cookieHelper.SetResponseAnalyticsCookieState(httpContext, AnalyticsConsentState.NotSet);

        // assert
        httpContext.Response.Cookies.Received().Delete(CookieHelper.ANALYTICSCOOKIENAME);
    }

    [Test]
    public void SetResponseAnalyticsCookieState_Sets_Secure_Cookie_Options()
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();
        CookieOptions cookieOptions = null;
        httpContext.Response.Cookies.Append(CookieHelper.ANALYTICSCOOKIENAME, CookieHelper.ANALYTICSCOOKIEACCEPTED, Arg.Do<CookieOptions>(x => cookieOptions = x));

        // act
        _cookieHelper.SetResponseAnalyticsCookieState(httpContext, AnalyticsConsentState.Accepted);

        // assert
        cookieOptions.Should().NotBeNull();
        cookieOptions.HttpOnly.Should().BeTrue();
        cookieOptions.SameSite.Should().Be(SameSiteMode.Strict);
        cookieOptions.IsEssential.Should().BeTrue();
        cookieOptions.Secure.Should().BeTrue();
    }

    [TestCase(CookieHelper.ANALYTICSCOOKIEACCEPTED, AnalyticsConsentState.Accepted)]
    [TestCase(CookieHelper.ANALYTICSCOOKIEREJECTED, AnalyticsConsentState.Rejected)]
    [TestCase("", AnalyticsConsentState.NotSet)]
    [TestCase(null, AnalyticsConsentState.NotSet)]
    [TestCase("Invalid value", AnalyticsConsentState.NotSet)]
    public void GetRequestAnalyticsCookieState(string cookieValue, AnalyticsConsentState expectedState)
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Cookies[CookieHelper.ANALYTICSCOOKIENAME].Returns(cookieValue);

        // act
        var result = _cookieHelper.GetRequestAnalyticsCookieState(httpContext);

        // assert
        result.Should().Be(expectedState);
    }

    [Test]
    public void Disables_Secure_Cookies_On_Config()
    {
        // arrange
        _applicationConfiguration.DisableSecureCookies.Returns(new BooleanConfigSetting(() => "false"));
        var httpContext = Substitute.For<HttpContext>();
        CookieOptions cookieOptions = null;
        httpContext.Response.Cookies.Append(CookieHelper.ANALYTICSCOOKIENAME, CookieHelper.ANALYTICSCOOKIEACCEPTED, Arg.Do<CookieOptions>(x => cookieOptions = x));

        // act
        _cookieHelper.SetResponseAnalyticsCookieState(httpContext, AnalyticsConsentState.Accepted);

        // assert
        cookieOptions.Should().NotBeNull();
        cookieOptions.HttpOnly.Should().BeTrue();
        cookieOptions.SameSite.Should().Be(SameSiteMode.Strict);
        cookieOptions.IsEssential.Should().BeTrue();
        cookieOptions.Secure.Should().BeTrue();
    }
}
