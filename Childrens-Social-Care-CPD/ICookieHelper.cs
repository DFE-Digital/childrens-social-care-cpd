namespace Childrens_Social_Care_CPD;

public interface ICookieHelper
{
    void SetResponseAnalyticsCookieState(HttpContext httpContext, AnalyticsConsentState state);
    AnalyticsConsentState GetRequestAnalyticsCookieState(HttpContext httpContext);
}
