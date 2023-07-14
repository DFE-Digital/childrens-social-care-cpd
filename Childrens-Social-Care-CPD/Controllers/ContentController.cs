using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Interfaces;
using Contentful.Core;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class ContentController : Controller
    {
        private readonly IContentfulClient _client;
        private readonly IContentfulDataService _dataService;
        private static int ContentFetchDepth = 10;
        private static string ContentTypeId = "content";
        private static string DefaultHomePageName = "home";

        public ContentController(ICpdContentfulClient client, IContentfulDataService dataService)
        {
            _client = client;
            _dataService = dataService;
        }

        private async Task FetchCookieContentIfRequiredAsync()
        {
            if (!Request.Cookies.ContainsKey(SiteConstants.ANALYTICSCOOKIENAME))
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

            var result = await _client.GetEntries(queryBuilder);

            return result?.FirstOrDefault();
        }

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
        public async Task<IActionResult> Index(string pageName)
        {
            await FetchCookieContentIfRequiredAsync();
            var pageContent = await FetchPageContentAsync(pageName);

            if (pageContent == null)
            {
                return NotFound();
            }
            
            ViewData["Title"] = pageContent.Title;
            ViewData["PageName"] = pageName;
            // Used to track circular dependencies in Content items and prevent overflow.
            ViewData["ContentStack"] = new Stack<string>();
            ViewData["UseContentContainers"] = true;
            
            return View(pageContent);
        }
    }
}