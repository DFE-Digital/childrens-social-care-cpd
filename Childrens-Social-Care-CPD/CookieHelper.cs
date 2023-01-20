using Childrens_Social_Care_CPD.Constants;

namespace Childrens_Social_Care_CPD
{
    public static class CookieHelper
    {
        public static void SetAnalyticsCookie(string analyticsCookieConsent, HttpContext httpContext)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(28),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true,
                Secure = true
            };

            httpContext.Response.Cookies.Append(SiteConstants.ANALYTICSCOOKIENAME, analyticsCookieConsent,
                options);
        }
    }
}
