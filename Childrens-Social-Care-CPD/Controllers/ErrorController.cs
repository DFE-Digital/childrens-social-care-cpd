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
        /// Unhandled exceptions in the pipeline are sent to this action.
        /// </summary>
        /// <returns>A StatusCodeResult of 500, not designed to be seen by the user.</returns>
        [HttpGet]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext?.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError(exceptionHandlerPathFeature?.Error, "Unhandled exception occurred");
            
            return StatusCode(500);
        }

        /// <summary>
        /// Error status codes returned by the pipeline are sent to this action in the same pipeline execution.
        /// </summary>
        /// <param name="code">The Http error code associated with the error.</param>
        /// <returns>The View for the specified error.</returns>
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            ViewData["pageName"] = $"error/{code}";
            switch (code)
            {
                case 404: return View("404");
                default : return View("500");
            }
        }
    }
}