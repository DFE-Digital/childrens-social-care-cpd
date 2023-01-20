
using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class BaseController : Controller
    {
        private readonly IContentfulClient _client;

        public BaseController(IContentfulClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Action filter to get Header and Footer contents
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.pageName = filterContext.ActionArguments.ContainsKey(SiteConstants.PAGENAME) ? filterContext.ActionArguments[SiteConstants.PAGENAME] ?? string.Empty : string.Empty;
            ViewBag.pageType = filterContext.ActionArguments.ContainsKey(SiteConstants.PAGETYPE) ? filterContext.ActionArguments[SiteConstants.PAGETYPE] ?? string.Empty : string.Empty;
            ViewBag.sendingPage = filterContext.ActionArguments.ContainsKey(SiteConstants.SENDINGPAGE) ? filterContext.ActionArguments[SiteConstants.SENDINGPAGE] ?? string.Empty : string.Empty;
            ViewBag.sendingPageType = filterContext.ActionArguments.ContainsKey(SiteConstants.SENDINGPAGETYPE) ? filterContext.ActionArguments[SiteConstants.SENDINGPAGETYPE] ?? string.Empty : string.Empty;

            PageHeader pageHeader = GetHeader();

            ViewBag.PageHeader = pageHeader;

            PageFooter pageFooter = GetFooter();

            ViewBag.PageFooter = pageFooter;

            var acceptsAnalytics = HttpContext?.Request.Cookies[SiteConstants.ANALYTICSCOOKIENAME]?.Equals("accept");

            if (acceptsAnalytics == null)
            {
                CookieBanner cookieBanner = GetCookieBanner();
                ViewBag.CookieBanner = cookieBanner;
            }
        }

       

        /// <summary>
        /// Method to get Footer using Contentful API call
        /// </summary>
        /// <returns></returns>
        private PageFooter GetFooter()
        {
            var footerQueryBuilder = QueryBuilder<PageFooter>.New.ContentTypeIs(SiteConstants.PAGEFOOTER);
            var footerResult = _client.GetEntries<PageFooter>(footerQueryBuilder).Result;
            var footer = footerResult.FirstOrDefault();
            PageFooter pageFooter = new PageFooter();

            if(footer!= null)
            {
                var htmlRenderer = new HtmlRenderer();

                pageFooter.FooterLinks = footer.FooterLinks.OrderBy(x => x.SortOrder).ToList();
                pageFooter.CopyrightLink = footer.CopyrightLink;
                pageFooter.LicenceDescriptionText = htmlRenderer.ToHtml(footer.LicenceDescription).Result;
            }
           
            return pageFooter;
        }

        /// <summary>
        /// Method to get Header using Contentful API call
        /// </summary>
        /// <returns></returns>
        private PageHeader GetHeader()
        {
            var queryBuilder = QueryBuilder<PageHeader>.New.ContentTypeIs(SiteConstants.PAGEHEADER);
            var result = _client.GetEntries<PageHeader>(queryBuilder).Result;
            var header = result.FirstOrDefault();
            var htmlRenderer = new HtmlRenderer();
            var html = htmlRenderer.ToHtml(header?.PrototypeText).Result;

            PageHeader pageHeader = new PageHeader
            {
                Header =  header == null ? string.Empty : header.Header,
                PrototypeTextHtml = html,
                PrototypeHeader = header == null ? string.Empty : header.PrototypeHeader,
                HeaderLinkTitle = header.HeaderLinkTitle            };
            return pageHeader;
        }

        /// <summary>
        /// To get contents for Cookie banner
        /// </summary>
        /// <returns></returns>
        private CookieBanner GetCookieBanner()
        {
            var cookieBannerQueryBuilder = QueryBuilder<CookieBanner>.New.ContentTypeIs(SiteConstants.COOKIEBANNER);
            var cookieBannerResult = _client.GetEntries<CookieBanner>(cookieBannerQueryBuilder).Result;
            var cookieBanner = cookieBannerResult.FirstOrDefault();
            return cookieBanner;
        }
    }
}
