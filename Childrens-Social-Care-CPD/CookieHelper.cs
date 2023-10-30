using Childrens_Social_Care_CPD.Configuration;

namespace Childrens_Social_Care_CPD;

public class CookieHelper: ICookieHelper
{
    public const string ANALYTICSCOOKIENAME = "cookie_consent";
    public const string ANALYTICSCOOKIEACCEPTED = "accept";
    public const string ANALYTICSCOOKIEREJECTED = "reject";
    
    private readonly IApplicationConfiguration _applicationConfiguration;

    public CookieHelper(IApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    public void SetResponseAnalyticsCookieState(HttpContext httpContext, AnalyticsConsentState state)
    {
        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365),
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Secure = !_applicationConfiguration.DisableSecureCookies.Value
        };

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

    public AnalyticsConsentState GetRequestAnalyticsCookieState(HttpContext httpContext)
    {
        var cookie = httpContext.Request.Cookies[ANALYTICSCOOKIENAME];
        return AnalyticsConsentStateHelper.Parse(cookie);
    }
}
