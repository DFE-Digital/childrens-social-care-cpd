using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class ContentController : Controller
    {
        private readonly ICpdContentfulClient _cpdClient;
        private readonly IContentfulDataService _dataService;
        private static int ContentFetchDepth = 10;
        private static string ContentTypeId = "content";
        private static string DefaultHomePageName = "home";

        public ContentController(ICpdContentfulClient cpdClient, IContentfulDataService dataService)
        {
            _cpdClient = cpdClient;
            _dataService = dataService;
        }

        private async Task FetchCookieContentIfRequiredAsync()
        {
            if (HttpContext.GetRequestAnalyticsCookieState() == AnalyticsConsentState.NotSet)
            {
                var cookieBannerData = await _dataService.GetCookieBannerData();
                ViewData["CookieBanner"] = cookieBannerData;
            }
        }

        private async Task<Content> FetchPageContentAsync(string contentId)
        {
            var queryBuilder = QueryBuilder<Content>.New
                .ContentTypeIs(ContentTypeId)
                .FieldEquals("fields.id", contentId ?? DefaultHomePageName)
                .Include(ContentFetchDepth);

            var result = await _cpdClient.GetEntries(queryBuilder);

            return result?.FirstOrDefault();
        }

        [HttpGet]
        [Route("content/")]
        /*
            Filter permissable page name format. Basically only accept:
                foo
                foo/bar
                foo/bar/xyz
            Reject
                /foo
                foo'bar
                Foo
                <=
            Etc.
        */
        [Route("content/{*pagename:regex(^[[0-9a-z]](\\/?[[0-9a-z\\-]])*\\/?$)}")] 
        public async Task<IActionResult> Index(string pageName, bool preferenceSet = false)
        {
            await FetchCookieContentIfRequiredAsync();
            var pageContent = await FetchPageContentAsync(pageName);
            if (pageContent == null)
            {
                return NotFound();
            }

            var contextModel = new ContextModel(
                Id: pageContent.Id, 
                Title: pageContent.Title, 
                PageName: pageName, 
                Category: pageContent.Category,
                UseContainers: pageContent.SideMenu == null, 
                PreferenceSet: preferenceSet, 
                BackLink: pageContent.BackLink);

            ViewData["ContextModel"] = contextModel;
            return View(pageContent);
        }
    }
}