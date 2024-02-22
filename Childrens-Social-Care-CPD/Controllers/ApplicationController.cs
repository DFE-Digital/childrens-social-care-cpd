using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Childrens_Social_Care_CPD.Controllers;

public class ApplicationController : Controller
{
    private readonly IApplicationConfiguration _applicationConfiguration;

    public ApplicationController(IApplicationConfiguration applicationConfiguration)
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

    [HttpGet]
    [Route("application/configuration")]
    public IActionResult Configuration()
    {
        var configurationInformation = new ConfigurationInformation(_applicationConfiguration);
        
        if (Request.Headers.Accept == MediaTypeNames.Application.Json)
        {
            var info = configurationInformation.ConfigurationInfo.Where(x => !x.Hidden);
            return Json(info.Select(x => new { x.Name, x.Extraneous, x.IsSet, x.Value, x.Obfuscated }));
        }

        return View(configurationInformation.ConfigurationInfo);
    }
}