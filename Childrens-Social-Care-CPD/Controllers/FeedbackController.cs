using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD.Controllers;

public class FeedbackModel
{
    public string Page { get; set; }
    public bool? IsUseful { get; set; }
    public string Comments { get; set; }
}

public class FeedbackController : Controller
{
    private readonly IFeaturesConfig _featuresConfig;
    private readonly ICpdContentfulClient _cpdClient;

    public FeedbackController(IFeaturesConfig featuresConfig, ICpdContentfulClient cpdClient)
    {
        ArgumentNullException.ThrowIfNull(featuresConfig);

        _featuresConfig = featuresConfig;
        _cpdClient = cpdClient;
    }

    private async Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .FieldEquals("fields.id", contentId)
            .Include(10);

        var result = await _cpdClient.GetEntries(queryBuilder, cancellationToken);

        return result?.FirstOrDefault();
    }

    private static bool IsModelValid(FeedbackModel model, out string pageId)
    {
        pageId = model.Page ?? string.Empty;
        pageId = pageId.Trim('/');
        
        if (pageId.Length > 512
            || model.Comments?.Length > 500 
            || !Regex.IsMatch(pageId, @"^[0-9a-z](\/?[0-9a-z\-])*\/?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1)))
        {
            return false; 
        }

        return true;
    }

    [HttpPost]
    [Route("feedback")]
    public async Task<IActionResult> Feedback([FromForm]FeedbackModel feedback, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.FeedbackControl))
        {
            return NotFound();
        }

        // Validate the page id
        if (!IsModelValid(feedback, out var pageId))
        {
            return BadRequest();
        }

        // Check the page exists
        var content = await FetchPageContentAsync(pageId, cancellationToken);
        if (content == null)
        {
            return BadRequest();
        }

        // TODO: do something with the feedback
        
        return Redirect($"~/{content.Id}?fs=true");
    }

    [HttpPost]
    [Route("api/feedback")]
    public async Task<IActionResult> JsonFeedback([FromBody]FeedbackModel feedback, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.FeedbackControl))
        {
            return NotFound();
        }

        // Validate the page id
        if (!IsModelValid(feedback, out var pageId))
        {
            return BadRequest();
        }

        // Check the page exists
        var content = await FetchPageContentAsync(pageId, cancellationToken);
        if (content == null)
        {
            return BadRequest();
        }

        // TODO: do something with the feedback

        return Ok();
    }
}