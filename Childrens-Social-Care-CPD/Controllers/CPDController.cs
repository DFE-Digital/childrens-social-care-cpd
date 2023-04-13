using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.ActionFilters;

namespace Childrens_Social_Care_CPD.Controllers
{
    [ServiceFilter(typeof(CPDActionFilter))]
    public class CPDController: Controller
    {
        private readonly ILogger<CPDController> _logger;
        private readonly IContentfulDataService _contentfulDataService;

        public CPDController(ILogger<CPDController> logger, IContentfulDataService contentfulDataService) 
        {
            _logger = logger;
            _contentfulDataService = contentfulDataService;
        }

        /// <summary>
        /// Common method to return any view (page).
        /// </summary>
        /// <param name="pageName">Application page name</param>
        /// <param name="pageType">Application page type</param>
        /// <param name="sendingPage">Previous page</param>
        /// <param name="sendingPageType">Previous page type</param>
        /// <returns>View with required view model based on page type and page name</returns>
        [HttpGet]
        public async Task<IActionResult> LandingPage(string pageName, string pageType, string sendingPage, string sendingPageType)
        {
            var pageViewModel = await _contentfulDataService.GetViewData<PageViewModel>(pageName, pageType);
            return View(pageViewModel);
        }
    }
}