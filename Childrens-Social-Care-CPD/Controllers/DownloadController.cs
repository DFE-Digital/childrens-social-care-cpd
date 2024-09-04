using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Contentful;
using Microsoft.Net.Http.Headers;

namespace Childrens_Social_Care_CPD.Controllers;

public class DownloadController(ICpdContentfulClient cpdClient) : Controller
{
    [Route("download/{assetId}")]
    [HttpGet]
    public async Task<IActionResult> Download(string assetId)
    {
        var httpClient = new HttpClient();
        try {
            var asset = await cpdClient.GetAsset(assetId);

            var cd = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = asset.File.FileName
            };
            Response.Headers.Append(HeaderNames.ContentDisposition, cd.ToString());

            var result = await httpClient.GetAsync("http:" + asset.File.Url);
            var stream = await result.Content.ReadAsStreamAsync();
            
            return File(stream, asset.File.ContentType);
        }
        catch (Exception ex) {
            return NotFound();
        }
    }
}