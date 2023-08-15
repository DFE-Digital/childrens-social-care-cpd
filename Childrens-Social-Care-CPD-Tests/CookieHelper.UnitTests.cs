using Childrens_Social_Care_CPD;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests;

public class CookieHelperTests
{
    [Test]
    [TestCase(AnalyticsConsentState.Accepted, SiteConstants.ANALYTICSCOOKIEACCEPTED)]
    [TestCase(AnalyticsConsentState.Rejected, SiteConstants.ANALYTICSCOOKIEREJECTED)]
    public void SetResponseAnalyticsCookieState_Sets_Correct_Value(AnalyticsConsentState state, string expectedValue)
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();

        // act
        httpContext.SetResponseAnalyticsCookieState(state);

        // assert
        httpContext.Response.Cookies.Received().Append(SiteConstants.ANALYTICSCOOKIENAME, expectedValue, Arg.Any<CookieOptions>());
    }

    [Test]
    public void SetResponseAnalyticsCookieState_Clears_Cookie()
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();

        // act
        httpContext.SetResponseAnalyticsCookieState(AnalyticsConsentState.NotSet);

        // assert
        httpContext.Response.Cookies.Received().Delete(SiteConstants.ANALYTICSCOOKIENAME);
    }

    [Test]
    public void SetResponseAnalyticsCookieState_Sets_Secure_Cookie_Options()
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();
        CookieOptions cookieOptions = null;
        httpContext.Response.Cookies.Append(SiteConstants.ANALYTICSCOOKIENAME, SiteConstants.ANALYTICSCOOKIEACCEPTED, Arg.Do<CookieOptions>(x => cookieOptions = x));

        // act
        httpContext.SetResponseAnalyticsCookieState(AnalyticsConsentState.Accepted);

        // assert
        cookieOptions.Should().NotBeNull();
        cookieOptions.HttpOnly.Should().BeTrue();
        cookieOptions.SameSite.Should().Be(SameSiteMode.Strict);
        cookieOptions.IsEssential.Should().BeTrue();
        cookieOptions.Secure.Should().BeTrue();
    }

    [Test]
    [TestCase(SiteConstants.ANALYTICSCOOKIEACCEPTED, AnalyticsConsentState.Accepted)]
    [TestCase(SiteConstants.ANALYTICSCOOKIEREJECTED, AnalyticsConsentState.Rejected)]
    [TestCase("", AnalyticsConsentState.NotSet)]
    [TestCase(null, AnalyticsConsentState.NotSet)]
    [TestCase("Invalid value", AnalyticsConsentState.NotSet)]
    public void GetRequestAnalyticsCookieState(string cookieValue, AnalyticsConsentState expectedState)
    {
        // arrange
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Cookies[SiteConstants.ANALYTICSCOOKIENAME].Returns(cookieValue);

        // act
        var result = httpContext.GetRequestAnalyticsCookieState();

        // assert
        result.Should().Be(expectedState);
    }
}
