using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Search;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace Childrens_Social_Care_CPD.Controllers;

public class SearchResourcesController : Controller
{
    private const string SearchRoute = "x";
    private const int PageSize = 8;
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

    [Route(SearchRoute)]
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

    private static string GetPagingFormatString(string searchTerm, IEnumerable<string> tags, SortOrder sortOrder)
    {
        var result = $"/{SearchRoute}?page={{0}}";

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

        if (sortOrder != SortOrder.UpdatedLatest)
        {
            Append(sortOrder.ToString(), "sortOrder");
        }
        Append(searchTerm, "term");
        Append(string.Join('&', tags.Select(x => $"tags={x}")));
        return result;
    }

    private async Task<ResourceSearchResultsViewModel> GetSearchModel(SearchRequest request, CancellationToken cancellationToken)
    {
        // Kick off our page content query
        var rootPageTask = _resourcesRepository.FetchRootPageAsync(cancellationToken);

        // Get the available tags and validate the ones we've been given
        var tagInfos = await _resourcesRepository.GetSearchTagsAsync(cancellationToken);
        var validTags = request.Tags?.Where(x => tagInfos.Any(y => y.TagName == x)) ?? Array.Empty<string>();
        
        // Create our search query
        var query = GetQuery(request, validTags);

        // Run the search and build our view model
        var searchResults = await _searchService.SearchResourcesAsync(query);
        var totalPages = (int)Math.Ceiling((decimal)(searchResults.Value.TotalCount ?? 0) / query.PageSize);
        var currentPage = totalPages == 0 ? 0 : Math.Clamp(query.Page, 1, totalPages);
        var facetedTags = GetFacetedTags(tagInfos, searchResults.Value.Facets["Tags"]);

        // Calculate the start and end result numbers
        var startResult = (currentPage - 1) * PageSize + 1;
        var endResult = Math.Min(startResult + PageSize - 1, searchResults.Value.TotalCount ?? 0);
        
        // Wait for the page content query to complete
        var pageContent = await rootPageTask;

        return new ResourceSearchResultsViewModel(
            pageContent,
            query.Keyword,
            searchResults.Value.TotalCount ?? 0,
            totalPages,
            currentPage,
            startResult,
            endResult,
            tagInfos,
            searchResults.Value.GetResults(),
            facetedTags,
            validTags,
            GetClearFiltersUri(request),
            GetPagingFormatString(query.Keyword, validTags, request.SortOrder),
            request.SortOrder);
    }

    private string GetClearFiltersUri(SearchRequest request)
    {
        var result = $"/{SearchRoute}?term={request.Term}";
        if (request.SortOrder != SortOrder.UpdatedLatest)
        {
            result += $"&sortOrder={request.SortOrder}";
        }
        
        return result;
    }

    private KeywordSearchQuery GetQuery(SearchRequest request, IEnumerable<string> validTags)
    {
        var page = Math.Max(request.Page, 1);
        var filter = new Dictionary<string, IEnumerable<string>> { { "Tags", validTags } };

        var sortCategory = request.SortOrder switch
        {
            SortOrder.MostRelevant => SortCategory.Relevancy,
            _ => SortCategory.Updated,
        };

        var sortDirection = request.SortOrder switch
        {
            SortOrder.UpdatedOldest => SortDirection.Ascending,
            _ => SortDirection.Descending,
        };

        return new KeywordSearchQuery(request.Term, page, PageSize, filter, sortCategory, sortDirection);
    }
}

public enum SortOrder
{
    UpdatedLatest,
    UpdatedOldest,
    MostRelevant,
}

public record SearchRequest(
    string Term,
    string[] Tags,
    int Page = 1,
    SortOrder SortOrder = SortOrder.UpdatedLatest);

public record ResourceSearchResultsViewModel(
    Content PageContent,
    string SearchTerm,
    long TotalCount,
    int TotalPages,
    int CurrentPage,
    long StartResultCount,
    long EndResultCount,
    IEnumerable<TagInfo> Tags,
    IEnumerable<SearchResult<CpdDocument>> SearchResults,
    IReadOnlyDictionary<TagInfo, FacetResult> FacetedTags,
    IEnumerable<string> SelectedTags,
    string ClearFiltersUri,
    string PagingFormatString,
    SortOrder SortOrder);