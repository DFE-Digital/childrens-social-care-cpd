using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.ActionFilters;

namespace Childrens_Social_Care_CPD.Controllers
{
    [ServiceFilter(typeof(CPDActionFilter))]
    public class CookieController : Controller
    {
        private readonly ILogger<CookieController> _logger;
        private readonly IContentfulDataService _contentfulDataService;

        public CookieController(ILogger<CookieController> logger, IContentfulDataService contentfulDataService) 
        {
            _logger = logger;
            _contentfulDataService = contentfulDataService;
        }

        /// <summary>
        /// Method to set Google analytics cookie consent from user
        /// </summary>
        /// <param name="analyticsCookieConsent">
        /// Cookie consent - Accept or Reject
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LandingPage(string analyticsCookieConsent, string pageName, string pageType, string referer, string sendingPageType, string sendingPage)
        {
            CookieHelper.SetAnalyticsCookie(analyticsCookieConsent, HttpContext);
            ViewBag.analyticsCookieSet = analyticsCookieConsent;
            ViewBag.Referer = referer;
            var pageViewModel = await _contentfulDataService.GetViewData<PageViewModel>(pageName, pageType);
            return View("~/Views/CPD/LandingPage.cshtml", pageViewModel);
        }

        /// <summary>
        /// Method to accept post and redirect to get method
        /// </summary>
        /// <param name="analyticsCookieConsent">
        /// Cookie consent - Accept or Reject
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetCookies(string analyticsCookieConsent, string pageName, string pageType, string referer, string sendingPageType, string sendingPage)
        {
            return RedirectToAction("LandingPage", new { analyticsCookieConsent, pageName, pageType, referer, sendingPageType, sendingPage });
        }
    }
}