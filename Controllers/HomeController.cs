using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContentfulClient _client;

        public HomeController(ILogger<HomeController> logger, IContentfulClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            var products = _client.GetEntries<Product>().Result;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}