using Childrens_Social_Care_CPD.Interfaces;
using Contentful.Core;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class ContentController : Controller
    {
        private readonly IContentfulClient _client;
        private readonly IContentfulDataService _dataService;

        public ContentController(IContentfulClient client, IContentfulDataService dataService)
        {
            _client = client;
            _dataService = dataService;
        }

        [Route("content/")]
        [Route("content/{*pagename:regex(^[[0-9a-z]](\\/?[[0-9a-z\\-]])*\\/?$)}")] // filter permissable page name format
        public IActionResult Index(string pageName)
        {
            return View();
        }
    }
}