using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public class AppInfoController : Controller
{
    private readonly IApplicationConfiguration _applicationConfiguration;

    public AppInfoController(IApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    [Route("CPD/AppInfo")]
    public JsonResult AppInfo()
    {
        var applicationInfo = new ApplicationInfo()
        {
            Environment = _applicationConfiguration.AzureEnvironment,
            ContentfulEnvironment = _applicationConfiguration.ContentfulEnvironment,
            GitShortHash = _applicationConfiguration.GitHash,
            Version = _applicationConfiguration.AppVersion,
        };

        return Json(applicationInfo);
    }
}