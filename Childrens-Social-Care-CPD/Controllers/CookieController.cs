using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.ActionFilters;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Search;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Constants;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Specialized;

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

        #region Utility functions

        private string SanitiseSourcePageUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri relativeUri) && Url.IsLocalUrl(url))
            {
                return relativeUri.ToString();
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri absoluteUri) &&
                absoluteUri.Authority == new Uri(Request.GetDisplayUrl()).Authority)
            {
                return absoluteUri.ToString();
            }

            return Url.Action("Index", "Content");
        }

        private Uri MakeAbsoluteUri(Uri uri)
        {
            if (uri.IsAbsoluteUri) return uri;

            var hostUri = new Uri(Request.GetDisplayUrl());
            return new Uri(new Uri($"{hostUri.Scheme}{Uri.SchemeDelimiter}{hostUri.Authority}"), uri);
        }

        private static Uri BuildRedirectUri(Uri uri, AnalyticsConsentState consentState)
        {
            if (consentState == AnalyticsConsentState.NotSet) return uri;

            var query = HttpUtility.ParseQueryString(uri.Query);
            query["preferenceSet"] = "true";

            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = query.ToString();

            return new Uri(uriBuilder.ToString());
        }

        #endregion

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

        [HttpPost]
        public IActionResult SetPreferences(string consentValue, string redirectTo = null)
        {
            var consentState = AnalyticsConsentStateHelper.Parse(consentValue);
            HttpContext.SetResponseAnalyticsCookieState(consentState);

            var url = redirectTo ?? Request.Headers.Referer;

            if (Uri.TryCreate(url, UriKind.Relative, out Uri relativeUri) && Url.IsLocalUrl(url))
            {
                relativeUri = BuildRedirectUri(MakeAbsoluteUri(relativeUri), consentState);
                return new LocalRedirectResult(relativeUri.PathAndQuery);
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri absoluteUri) &&
                absoluteUri.Authority == new Uri(Request.GetDisplayUrl()).Authority)
            {
                return new RedirectResult(BuildRedirectUri(absoluteUri, consentState).ToString());
            }

            return consentState == AnalyticsConsentState.NotSet
                    ? new LocalRedirectResult(Url.Action("Index", "Content"))
                    : new LocalRedirectResult(Url.Action("Index", "Content", new { preferenceSet = "true" }));
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

            var contextModel = new ContextModel(pageContent.Id, pageContent.Title, PageName, pageContent.Category, true, preferenceSet, true);
            ViewData["ContextModel"] = contextModel;

            var model = new CookiesAndAnalyticsConsentModel
            {
                Content = pageContent,
                ConsentState = consentState,
                SourceUrl = SanitiseSourcePageUrl(sourcePage),
            };

            return View(model);
        }
    }
}