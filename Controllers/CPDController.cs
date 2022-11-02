using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System;
using Childrens_Social_Care_CPD.Enums;

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
        
        public async Task<IActionResult> LandingPage(string PageName, string PageType)
        {
            PageViewModel pageViewModel = await GetViewModel(PageName, PageType);
            return View(pageViewModel);
        }

        private async Task<PageViewModel> GetViewModel(string pageName, string PageType)
        {
            var pageViewModel = new PageViewModel();
            if(string.IsNullOrEmpty(pageName) && string.IsNullOrEmpty(PageType))
            {
                pageName = PageNames.HomePage.ToString();
                pageViewModel.PageType = PageTypes.Master;
            }
            else 
            {
                PageTypes pageType = (PageTypes)Enum.Parse(typeof(PageTypes), PageType);
                pageViewModel.PageType = pageType;
            }

            var queryBuilder = QueryBuilder<Role>.New.ContentTypeIs("section").FieldEquals("fields.rolePageName", pageName).OrderBy("fields.sortOrder");
            var result = await _client.GetEntries<Role>(queryBuilder);
           
            pageViewModel.Roles = result;

            var paragraphQueryBuilder = QueryBuilder<Childrens_Social_Care_CPD.Models.Paragraph>.New.ContentTypeIs("paragraph").FieldEquals("fields.paragraphPageName", pageName).OrderBy("fields.sortOrder");
            var paragraphsResult = await _client.GetEntries<Childrens_Social_Care_CPD.Models.Paragraph>(paragraphQueryBuilder);
            pageViewModel.Paragraphs = paragraphsResult;

            return pageViewModel;
        }
    }
}