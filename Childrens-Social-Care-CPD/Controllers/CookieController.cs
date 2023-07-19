using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.ActionFilters;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Search;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Constants;
using System.Web;

namespace Childrens_Social_Care_CPD.Controllers
{
    [ServiceFilter(typeof(CPDActionFilter))]
    public class CookieController : Controller
    {
        private readonly ILogger<CookieController> _logger;
        private readonly IContentfulDataService _contentfulDataService;
        private readonly ICpdContentfulClient _cpdClient;

        private static int ContentFetchDepth = 10;
        private static string ContentTypeId = "content";
        private static string PageName = "cookies";

        public CookieController(ILogger<CookieController> logger, IContentfulDataService contentfulDataService, ICpdContentfulClient cpdClient)
        {
            _logger = logger;
            _contentfulDataService = contentfulDataService;
            _cpdClient = cpdClient;
        }

        [HttpGet]
        public async Task<IActionResult> LandingPage(string analyticsCookieConsent, string pageName, string pageType, string referer, string sendingPageType, string sendingPage)
        {
            var consentState = AnalyticsConsentStateHelper.Parse(analyticsCookieConsent);

            HttpContext.SetResponseAnalyticsCookieState(consentState);
            ViewBag.analyticsCookieSet = analyticsCookieConsent;
            ViewBag.Referer = referer;
            var pageViewModel = await _contentfulDataService.GetViewData<PageViewModel>(pageName, pageType);

            return View("~/Views/CPD/LandingPage.cshtml", pageViewModel);
        }

        [HttpPost]
        public IActionResult SetCookies(string analyticsCookieConsent, string pageName, string pageType, string referer, string sendingPageType, string sendingPage)
        {
            return RedirectToAction("LandingPage", new { analyticsCookieConsent, pageName, pageType, referer, sendingPageType, sendingPage });
        }

        private string GetRedirectUrl(string referer, string redirectTo, string fallbackUrl, string host, AnalyticsConsentState consentState)
        {
            var url = redirectTo ?? referer;

            if (string.IsNullOrEmpty(url))
                url = fallbackUrl;
            
            var uri = new Uri(url);

            if (uri.Host != Request.Host.Host)
                url = fallbackUrl;

            if (consentState == AnalyticsConsentState.NotSet)
            {
                return url;
            }

            var query = HttpUtility.ParseQueryString(uri.Query);
            query[SiteConstants.PreferenceSet] = "true";

            var uriBuilder = new UriBuilder(url);
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        [HttpPost]
        public IActionResult SetPreferences(string consentValue, string redirectTo = null)
        {
            var consentState = AnalyticsConsentStateHelper.Parse(consentValue);

            HttpContext.SetResponseAnalyticsCookieState(consentState);

            var uri = GetRedirectUrl(
                Request.Headers.Referer,
                redirectTo,
                Url.Action("Index", "Content", null, Request.Scheme, Request.Host.Value),
                Request.Host.Value,
                consentState);

            return new RedirectResult(uri);
        }

        [HttpGet]
        [Route("cookies")]
        public async Task<IActionResult> Cookies(string sourcePage = null, bool preferenceSet = false)
        {
            var queryBuilder = QueryBuilder<Content>.New
                .ContentTypeIs(ContentTypeId)
                .FieldEquals("fields.id", PageName)
                .Include(ContentFetchDepth);

            var result = await _cpdClient.GetEntries(queryBuilder);
            var pageContent = result?.FirstOrDefault();
            
            if (pageContent == null)
            {
                return NotFound();
            }
            
            var consentState = HttpContext.GetRequestAnalyticsCookieState();

            ViewData[SiteConstants.PageTitle] = pageContent.Title;
            ViewData[SiteConstants.PageName] = PageName;
            ViewData[SiteConstants.ContentStack] = new Stack<string>();
            ViewData[SiteConstants.UseContainers] = true;
            ViewData[SiteConstants.PreferenceSet] = preferenceSet;
            ViewData[SiteConstants.HideConsent] = true; // We're on the cookie consent page so we don't need the banner dialogs

            var model = new CookiesAndAnalyticsConsentModel
            {
                Content = pageContent,
                ConsentState = consentState,
                SourceUrl = sourcePage ?? Url.Action("Index", "Content"),
                PreferencesSet = preferenceSet
            };

            return View(model);
        }
    }
}