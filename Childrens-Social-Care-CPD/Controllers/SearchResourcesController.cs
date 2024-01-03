using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public class SearchResourcesController : Controller
{
    private const string SearchRoute = "resources-learning";
    private const int PageSize = 8;
    private readonly IFeaturesConfig _featuresConfig;
    private readonly ISearchResultsVMFactory _searchResultsVMFactory;

    public SearchResourcesController(IFeaturesConfig featuresConfig, ISearchResultsVMFactory searchResultsVMFactory)
    {
        ArgumentNullException.ThrowIfNull(featuresConfig);
        _featuresConfig = featuresConfig;
        _searchResultsVMFactory = searchResultsVMFactory;
    }

    [Route(SearchRoute)]
    [HttpGet]
    public async Task<IActionResult> SearchResources([FromQuery] SearchRequestModel query, bool preferencesSet = false, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.ResourcesAndLearning))
        {
            return NotFound();
        }

        var contextModel = new ContextModel(string.Empty, "Resources and learning search", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = await _searchResultsVMFactory.GetSearchModel(query, PageSize, SearchRoute, cancellationToken);
        return View("SearchResources", viewModel);
    }
}