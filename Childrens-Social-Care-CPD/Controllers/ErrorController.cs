using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger) 
        {
            _logger = logger;
        }

        /// <summary>
        /// Application global exception handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext?.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"CPDException {exceptionHandlerPathFeature?.Error.Message}");
            
            ViewData["pageName"] = $"error/500";
            return StatusCode(500);
        }

        /// <summary>
        /// To hanndle error with specific error code for e.g. 404
        /// This method is invoked with the middleware
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            ViewData["pageName"] = $"error/{code}";

            switch (code)
            {
                case 404:
                case 500: return View(code.ToString());
                default : return View("500");
            }
        }
    }
}