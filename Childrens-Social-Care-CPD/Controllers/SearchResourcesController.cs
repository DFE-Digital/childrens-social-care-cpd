using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Search;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace Childrens_Social_Care_CPD.Controllers;

public class SearchResourcesController : Controller
{
    private readonly IFeaturesConfig _featuresConfig;
    private readonly ISearchService _searchService;
    private readonly IResourcesRepository _resourcesRepository;

    public SearchResourcesController(IFeaturesConfig featuresConfig, ISearchService searchService, IResourcesRepository resourcesRepository)
    {
        ArgumentNullException.ThrowIfNull(featuresConfig);
        ArgumentNullException.ThrowIfNull(searchService);
        _featuresConfig = featuresConfig;
        _searchService = searchService;
        _resourcesRepository = resourcesRepository;
    }

    [Route("x")]
    [HttpGet]
    public async Task<IActionResult> SearchResources([FromQuery] SearchRequest query, bool preferencesSet = false, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.ResourcesAndLearning))
        {
            return NotFound();
        }

        var contextModel = new ContextModel(string.Empty, "Resources and learning search", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = await GetSearchModel(query, cancellationToken);
        return View("SearchResources", viewModel);
    }

    private static IReadOnlyDictionary<TagInfo, FacetResult> GetFacetedTags(IEnumerable<TagInfo> tagInfos, IList<FacetResult> facets)
    {
        var map = facets.ToDictionary(x => x.Value as string, x => x);
        var tags = tagInfos.ToDictionary(tagInfo => tagInfo, tagInfo => map.GetValueOrDefault(tagInfo.TagName));

        return new ReadOnlyDictionary<TagInfo, FacetResult>(tags);
    }

    private static string GetPagingFormatString(string searchTerm, IEnumerable<string> tags, SortCategory sortCategory, SortDirection sortDirection)
    {
        var result = $"/search?page={{0}}";

        void Append(string param, string name = null)
        {
            if (string.IsNullOrEmpty(param))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(name))
            {
                result += $"&{param}";
            }
            else
            {
                result += $"&{name}={param}";
            }
        }

        Append(sortCategory.ToString(), "sortCategory");
        Append(sortDirection.ToString(), "sortDirection");
        Append(searchTerm, "term");
        Append(string.Join('&', tags.Select(x => $"tags={x}")));
        return result;
    }

    private async Task<ResourceSearchResultsViewModel> GetSearchModel(SearchRequest request, CancellationToken cancellationToken)
    {
        const int PageSize = 8;
        // Kick off our page content query
        var rootPageTask = _resourcesRepository.FetchRootPageAsync(cancellationToken);

        // Get the available tags and validate the ones we've been given
        var tagInfos = await _resourcesRepository.GetSearchTagsAsync(cancellationToken);

        var validTags = request.Tags?.Where(x => tagInfos.Any(y => y.TagName == x)) ?? Array.Empty<string>();
        
        // Create our search query
        var page = Math.Max(request.Page, 1);
        var filter = new Dictionary<string, IEnumerable<string>> { { "Tags", validTags } };
        var query = new KeywordSearchQuery(request.Term, page, PageSize, filter, request.SortCategory, request.SortDirection);

        // Run the search and build our view model
        var searchResults = await _searchService.SearchResourcesAsync(query);
        var totalPages = (int)Math.Ceiling((decimal)(searchResults.Value.TotalCount ?? 0) / query.PageSize);
        var currentPage = totalPages == 0 ? 0 : Math.Clamp(query.Page, 1, totalPages);
        var facetedTags = GetFacetedTags(tagInfos, searchResults.Value.Facets["Tags"]);

        // Wait for the page content query to complete
        var pageContent = await rootPageTask;

        return new ResourceSearchResultsViewModel(
            pageContent,
            query.Keyword,
            searchResults.Value.TotalCount ?? 0,
            totalPages,
            currentPage,
            tagInfos,
            searchResults.Value.GetResults(),
            facetedTags,
            validTags,
            GetPagingFormatString(query.Keyword, validTags, query.SortCategory, query.SortDirection),
            query.SortCategory,
            query.SortDirection);
    }
}

public record SearchRequest(
    string Term,
    string[] Tags,
    int Page = 1,
    SortCategory SortCategory = SortCategory.Relevancy,
    SortDirection SortDirection = SortDirection.Descending);


public record ResourceSearchResultsViewModel(
    Content PageContent,
    string SearchTerm,
    long TotalCount,
    int TotalPages,
    int CurrentPage,
    IEnumerable<TagInfo> Tags,
    IEnumerable<SearchResult<CpdDocument>> SearchResults,
    IReadOnlyDictionary<TagInfo, FacetResult> FacetedTags,
    IEnumerable<string> SelectedTags,
    string PagingFormatString,
    SortCategory SortCategory,
    SortDirection SortDirection);