using Childrens_Social_Care_CPD.Constants;

namespace Childrens_Social_Care_CPD
{
    public static class CookieHelper
    {
        public static void SetResponseAnalyticsCookieState(this HttpContext httpContext, AnalyticsConsentState state)
        {
            var secureCookie = Environment.GetEnvironmentVariable(SiteConstants.SECURECOOKIES)?.ToLower() == "false"
                ? false
                : true;

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true,
                Secure = secureCookie,
            };

            switch (state)
            {
                case AnalyticsConsentState.Accepted:
                    httpContext.Response.Cookies.Append(SiteConstants.ANALYTICSCOOKIENAME, SiteConstants.ANALYTICSCOOKIEACCEPTED, cookieOptions);
                    break;
                case AnalyticsConsentState.Rejected:
                    httpContext.Response.Cookies.Append(SiteConstants.ANALYTICSCOOKIENAME, SiteConstants.ANALYTICSCOOKIEREJECTED, cookieOptions);
                    break;
                case AnalyticsConsentState.NotSet:
                    httpContext.Response.Cookies.Delete(SiteConstants.ANALYTICSCOOKIENAME);
                    break;
            }
        }

        public static AnalyticsConsentState GetRequestAnalyticsCookieState(this HttpContext httpContext)
        {
            var cookie = httpContext.Request.Cookies[SiteConstants.ANALYTICSCOOKIENAME];
            return AnalyticsConsentStateHelper.Parse(cookie);
        }
    }
}
