using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class HomeController: Controller
    {
        private readonly ILogger<CPDController> _logger;
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
