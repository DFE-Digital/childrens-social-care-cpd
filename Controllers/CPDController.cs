using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Constants;

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
        /// <param name="PageName">Contentful page name</param>
        /// <param name="PageType">Contentful page type</param>
        /// <returns>View with required view model based on page type and page name</returns>
        public async Task<IActionResult> LandingPage(string PageName, string PageType)
        {
            ContentfulCollection<PageViewModel> pageViewModel = new ContentfulCollection<PageViewModel>();
            try
            {
                pageViewModel = await GetViewModel(PageName, PageType);
                foreach (PageViewModel viewModel in pageViewModel)
                {
                    viewModel.Cards = viewModel.Cards?.OrderBy(x => x.SortOrder).ToList();
                    viewModel.Paragraphs = viewModel.Paragraphs?.OrderBy(x => x.SortOrder).ToList();
                    viewModel.Labels = viewModel.Labels?.OrderBy(x => x.SortOrder).ToList();
                    viewModel.RichTexts = viewModel.RichTexts?.OrderBy(x => x.SortOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                // To-do - Logging (AppInsights?)
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

            var queryBuilder = QueryBuilder<PageViewModel>.New.ContentTypeIs(ContentTypes.PAGE)
                .FieldEquals("fields.pageName.fields.pageName", pageName)
                .FieldEquals("fields.pageName.sys.contentType.sys.id", ContentTypes.PAGENAMES)
                .Include(contentLevel);

            var result = await _client.GetEntries<PageViewModel>(queryBuilder);
            result.All(c => { c.PageType = contentPageType; return true; });
            
            return result;
        }
      
    }
}