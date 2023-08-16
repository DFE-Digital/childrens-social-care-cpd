namespace Childrens_Social_Care_CPD;

public static class CookieHelper
{
    public const string ANALYTICSCOOKIENAME = "cookie_consent";
    public const string ANALYTICSCOOKIEACCEPTED = "accept";
    public const string ANALYTICSCOOKIEREJECTED = "reject";

    public static void SetResponseAnalyticsCookieState(this HttpContext httpContext, AnalyticsConsentState state)
    {
        var options = new CookieOptions
        {
            var secureCookie = Environment.GetEnvironmentVariable(SiteConstants.DISABLESECURECOOKIES) != "true";
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true,
                Secure = secureCookie
            };
        }

        switch (state)
        {
            case AnalyticsConsentState.Accepted:
                httpContext.Response.Cookies.Append(ANALYTICSCOOKIENAME, ANALYTICSCOOKIEACCEPTED, options);
                break;
            case AnalyticsConsentState.Rejected:
                httpContext.Response.Cookies.Append(ANALYTICSCOOKIENAME, ANALYTICSCOOKIEREJECTED, options);
                break;
            case AnalyticsConsentState.NotSet:
                httpContext.Response.Cookies.Delete(ANALYTICSCOOKIENAME);
                break;
        }
    }

    public static AnalyticsConsentState GetRequestAnalyticsCookieState(this HttpContext httpContext)
    {
        var cookie = httpContext.Request.Cookies[ANALYTICSCOOKIENAME];
        return AnalyticsConsentStateHelper.Parse(cookie);
    }
}
