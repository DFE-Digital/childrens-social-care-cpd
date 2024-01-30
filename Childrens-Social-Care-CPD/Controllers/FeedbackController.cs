using Childrens_Social_Care_CPD.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD.Controllers;

public class FeedbackModel
{
    public string Page { get; set; }
    public bool IsUseful { get; set; }
    public string Comments { get; set; }
}

public class FeedbackController : Controller
{
    private readonly IFeaturesConfig _featuresConfig;

    public FeedbackController(IFeaturesConfig featuresConfig)
    {
        ArgumentNullException.ThrowIfNull(featuresConfig);

        _featuresConfig = featuresConfig;
    }

    private bool IsModelValid(FeedbackModel model, out string pageId)
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
    public IActionResult Feedback([FromForm]FeedbackModel feedback)
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

        // TODO: do something with the feedback
        
        return Redirect($"/{pageId}?fs=true");
    }

    [HttpPost]
    [Route("api/feedback")]
    public IActionResult JsonFeedback([FromBody]FeedbackModel feedback)
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

        // TODO: do something with the feedback

        return Ok();
    }
}
