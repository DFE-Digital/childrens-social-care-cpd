using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<ContentfulController> _logger;
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
