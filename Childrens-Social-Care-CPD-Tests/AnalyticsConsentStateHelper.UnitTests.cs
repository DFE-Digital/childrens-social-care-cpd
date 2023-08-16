using Childrens_Social_Care_CPD;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests;

public class AnalyticsConsentStateHelperTest
{
    [TestCase("", AnalyticsConsentState.NotSet)]
    [TestCase(CookieHelper.ANALYTICSCOOKIEREJECTED, AnalyticsConsentState.Rejected)]
    [TestCase(CookieHelper.ANALYTICSCOOKIEACCEPTED, AnalyticsConsentState.Accepted)]
    [TestCase(null, AnalyticsConsentState.NotSet)]
    [TestCase("invalid", AnalyticsConsentState.NotSet)]
    public void Parses_Value_Correctly(string input, AnalyticsConsentState expected)
    {
        // act
        var actual = AnalyticsConsentStateHelper.Parse(input);

        // assert
        actual.Should().Be(expected);
    }
}
