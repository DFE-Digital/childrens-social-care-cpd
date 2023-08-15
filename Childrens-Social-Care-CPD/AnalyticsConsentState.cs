namespace Childrens_Social_Care_CPD;

public enum AnalyticsConsentState
{
    NotSet,
    Accepted,
    Rejected,
}

public static class AnalyticsConsentStateHelper
{
    public static AnalyticsConsentState Parse(string value)
    {
        return value switch
        {
            SiteConstants.ANALYTICSCOOKIEACCEPTED => AnalyticsConsentState.Accepted,
            SiteConstants.ANALYTICSCOOKIEREJECTED => AnalyticsConsentState.Rejected,
            _ => AnalyticsConsentState.NotSet
        };
    }
}
