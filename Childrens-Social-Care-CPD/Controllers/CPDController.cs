using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Constants;
using Microsoft.AspNetCore.Diagnostics;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class CPDController : BaseController
    {
        private readonly ILogger<CPDController> _logger;
        private readonly IContentfulClient _client;

        public CPDController(ILogger<CPDController> logger, IContentfulClient client) :base(client)
        {
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Common method to return any view (page).
        /// </summary>
        /// <param name="pageName">Application page name</param>
        /// <param name="pageType">Application page type</param>
        /// <param name="sendingPage">Previous page</param>
        /// <param name="sendingPageType">Previous page type</param>
        /// <returns>View with required view model based on page type and page name</returns>
        public async Task<IActionResult> LandingPage(string pageName, string pageType, string sendingPage, string sendingPageType)
        {
            var pageViewModel = await GetViewModel(pageName, pageType);
            foreach (PageViewModel viewModel in pageViewModel)
            {
                viewModel.Cards = viewModel.Cards.OrderBy(x => x.SortOrder).ToList();
                viewModel.Labels = viewModel.Labels.OrderBy(x => x.SortOrder).ToList();
                viewModel.RichTexts = viewModel.RichTexts.OrderBy(x => x.SortOrder).ToList();
            }
            
            return View(pageViewModel);
        }

        /// <summary>
        /// Method to get contentful content using API call
        /// </summary>
        /// <param name="pageName">Contentful page name</param>
        /// <param name="pageType">Contentful page type</param>
        /// <returns></returns>
        private async Task<ContentfulCollection<PageViewModel>> GetViewModel(string pageName, string pageType)
        {
            int contentLevel = 10;
            ContentPageType contentPageType;

            if (string.IsNullOrEmpty(pageName) && string.IsNullOrEmpty(pageType))
            {
                pageName = PageNames.HomePage.ToString();
                contentPageType = new ContentPageType { PageType = PageTypes.Master.ToString() };
            }
            else
            {
                contentPageType = new ContentPageType { PageType = pageType };
            }

            var queryBuilder = QueryBuilder<PageViewModel>.New.ContentTypeIs(Constants.SiteConstants.PAGE)
                .FieldEquals("fields.pageName.fields.pageName", pageName)
                .FieldEquals("fields.pageName.sys.contentType.sys.id", Constants.SiteConstants.PAGENAMES)
                .Include(contentLevel);

            var result = await _client.GetEntries<PageViewModel>(queryBuilder);
            result.All(c => { c.PageType = contentPageType; return true; });
            
            return result;
        }

        /// <summary>
        /// Application global exception handler
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
                    Source = exceptionHandlerPathFeature?.Error.Source,
                    ErrorPath = exceptionHandlerPathFeature?.Path,
                    StackTrace = exceptionHandlerPathFeature?.Error.StackTrace,
                    InnerException = Convert.ToString(exceptionHandlerPathFeature?.Error.InnerException)
                }
                );
        }

    }
}