using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Childrens_Social_Care_CPD.Controllers;
/// comment 
public class ApplicationController(IApplicationConfiguration applicationConfiguration) : Controller
{
    [HttpGet]
    [Route("CPD/AppInfo")]
    public JsonResult AppInfo()
    {
        var applicationInfo = new ApplicationInfo()
        {
            Environment = applicationConfiguration.AzureEnvironment,
            ContentfulEnvironment = applicationConfiguration.ContentfulEnvironment,
            GitShortHash = applicationConfiguration.GitHash,
            Version = applicationConfiguration.AppVersion,
        };

        return Json(applicationInfo);
    }

    [HttpGet]
    [Route("application/configuration")]
    public IActionResult Configuration()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var configurationInformation = new ConfigurationInformation(applicationConfiguration);
        
        if (Request.Headers.Accept == MediaTypeNames.Application.Json)
        {
            var info = configurationInformation.ConfigurationInfo.Where(x => !x.Hidden);
            return Json(info.Select(x => new { x.Name, x.Extraneous, x.IsSet, x.Value, x.Obfuscated }));
        }

        return View(configurationInformation.ConfigurationInfo);
    }
}