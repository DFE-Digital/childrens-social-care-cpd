using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Constants;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class AppInfoController : Controller
    {
        public AppInfoController()
        {
        }

        /// <summary>
        /// Method to get Application information
        /// </summary>
        /// <returns>
        /// Application information
        /// </returns>
        [HttpGet]
        [Route("CPD/AppInfo")]
        public JsonResult AppInfo()
        {
            var applicationInfo = new ApplicationInfo()
            {
                Environment = Environment.GetEnvironmentVariable(SiteConstants.ENVIRONMENT) ?? String.Empty,
                ContentfulEnvironment = Environment.GetEnvironmentVariable(SiteConstants.AZUREENVIRONMENT) ?? String.Empty,
                GitShortHash = Environment.GetEnvironmentVariable(SiteConstants.VCSREF) ?? String.Empty
            };

            return Json(applicationInfo);
        }
    }
}