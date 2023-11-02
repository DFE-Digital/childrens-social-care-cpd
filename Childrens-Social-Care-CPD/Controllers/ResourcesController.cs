using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Extensions;

namespace Childrens_Social_Care_CPD.Controllers;

public enum ResourceSortOrder
{ 
    UpdatedNewest = 0,
    UpdatedOldest = 1
}

public class ResourcesQuery
{
    public string[] Tags { get; set; }
    public int Page { get; set; } = 1;
    public string? Order { get; set; }
    public ResourceSortOrder SortOrder { get; set; }

public ResourcesQuery()
    {
        Tags = Array.Empty<string>();
    }
}

public class ResourcesController : Controller
{
    private readonly IFeaturesConfig _featuresConfig;
    private readonly IResourcesSearchStrategy _strategy;

    public ResourcesController(IFeaturesConfig featuresConfig, IResourcesSearchStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        _featuresConfig = featuresConfig;
        _strategy = strategy;
    }

    [Route("resources-learning")]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ResourcesQuery query, bool preferencesSet = false, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.ResourcesAndLearning))
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(query.Order))
        {
            query.SortOrder = query.Order.ToEnum<ResourceSortOrder>();
        }

        var contextModel = new ContextModel(string.Empty, "Resources", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = await _strategy.SearchAsync(query, cancellationToken);
        return View(viewModel);
    }
}
