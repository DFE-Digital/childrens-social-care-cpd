using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Services;
using Contentful.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Childrens_Social_Care_CPD.ActionFilters
{
    public class CPDActionFilter: ActionFilterAttribute
    {
        private readonly ILogger<CPDActionFilter> _logger;
        private readonly IContentfulDataService _contentfulDataService;

        public CPDActionFilter(ILogger<CPDActionFilter> logger, IContentfulDataService contentfulDataService)
        {
            _contentfulDataService = contentfulDataService;
            _logger = logger;
        }

        /// <summary>
        /// Action filter to get Header and Footer contents
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                Controller controller = filterContext.Controller as Controller;
                controller.ViewBag.pageName = filterContext.ActionArguments.ContainsKey(SiteConstants.PAGENAME) ? filterContext.ActionArguments[SiteConstants.PAGENAME] ?? string.Empty : string.Empty;
                controller.ViewBag.pageType = filterContext.ActionArguments.ContainsKey(SiteConstants.PAGETYPE) ? filterContext.ActionArguments[SiteConstants.PAGETYPE] ?? string.Empty : string.Empty;
                controller.ViewBag.sendingPage = filterContext.ActionArguments.ContainsKey(SiteConstants.SENDINGPAGE) ? filterContext.ActionArguments[SiteConstants.SENDINGPAGE] ?? string.Empty : string.Empty;
                controller.ViewBag.sendingPageType = filterContext.ActionArguments.ContainsKey(SiteConstants.SENDINGPAGETYPE) ? filterContext.ActionArguments[SiteConstants.SENDINGPAGETYPE] ?? string.Empty : string.Empty;

                PageHeader pageHeader = _contentfulDataService.GetHeaderData().Result;

                controller.ViewBag.PageHeader = pageHeader;

                PageFooter pageFooter = _contentfulDataService.GetFooterData().Result;

                controller.ViewBag.PageFooter = pageFooter;

                var acceptsAnalytics = filterContext.HttpContext.Request.Cookies[SiteConstants.ANALYTICSCOOKIENAME]?.Equals("accept");

                if (acceptsAnalytics == null)
                {
                    CookieBanner cookieBanner = _contentfulDataService.GetCookieBannerData().Result;
                    controller.ViewBag.CookieBanner = cookieBanner;
                }

                if (controller.ViewBag.pageName == PageNames.ViewCookies.ToString())
                {
                    controller.ViewBag.Referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
