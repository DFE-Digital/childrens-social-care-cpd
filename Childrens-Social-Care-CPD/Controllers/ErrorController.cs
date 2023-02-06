using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Constants;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IContentfulClient _client;


        public ErrorController(ILogger<ErrorController> logger, IContentfulClient client) : base(client)
        {
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Application global exception handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext?.Features.Get<IExceptionHandlerPathFeature>();
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
                    Source = exceptionHandlerPathFeature?.Error.Source,
                    ErrorPath = exceptionHandlerPathFeature?.Path,
                    StackTrace = exceptionHandlerPathFeature?.Error.StackTrace,
                    InnerException = Convert.ToString(exceptionHandlerPathFeature?.Error.InnerException),
                    ErrorCode = HttpStatusCode.InternalServerError
                }
            );
        }

        /// <summary>
        /// To hanndle error with specific error code for e.g. 404
        /// This method is invoked with the middleware
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("Error/Error/{code:int}")]
        public IActionResult Error(int code) => View(
                new ErrorViewModel
                {
                    ErrorCode = (HttpStatusCode)code
                }
            );
    }
}