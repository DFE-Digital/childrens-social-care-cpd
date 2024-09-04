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
        // httpClient for fetching the asset file contents
        var httpClient = new HttpClient();

        try {
            // get the details of the asset from contentful
            var asset = await cpdClient.GetAsset(assetId);

            // set up a ContentDisposition header
            var cd = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = asset.File.FileName
            };
            
            // ... append it to the response headers
            Response.Headers.Append(HeaderNames.ContentDisposition, cd.ToString());

            // get the file contents (do we really not get the protocol part of the url?)
            var result = await httpClient.GetAsync("http:" + asset.File.Url);

            // read the file contents as a stream
            var stream = await result.Content.ReadAsStreamAsync();

            // stream the file to the user in the response
            return File(stream, asset.File.ContentType);
        }
        catch (Exception ex) {
            // will need to do some excetion sniffing here to make sure we're doing 404s and 500s appropriately
            return NotFound();
        }
    }
}