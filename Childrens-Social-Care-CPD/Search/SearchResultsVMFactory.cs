using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Models;
using System.Collections.ObjectModel;

namespace Childrens_Social_Care_CPD.Search;

public interface ISearchResultsVMFactory
{
    Task<ResourceSearchResultsViewModel> GetSearchModel(SearchRequestModel request, int pageSize, string routeName, CancellationToken cancellationToken);
}

internal class SearchResultsVMFactory : ISearchResultsVMFactory
{
    private readonly ISearchService _searchService;
    private readonly IResourcesRepository _resourcesRepository;

    public SearchResultsVMFactory(ISearchService searchService, IResourcesRepository resourcesRepository)
    {
        ArgumentNullException.ThrowIfNull(searchService);
        ArgumentNullException.ThrowIfNull(resourcesRepository);
        _searchService = searchService;
        _resourcesRepository = resourcesRepository;
    }

    private static IReadOnlyDictionary<TagInfo, FacetResult> GetFacetedTags(IEnumerable<TagInfo> tagInfos, IDictionary<string, IList<FacetResult>> facets)
    {
        facets.TryGetValue("Tags", out var tagFacets);
        if (tagFacets == null)
        {
            return new ReadOnlyDictionary<TagInfo, FacetResult>(new Dictionary<TagInfo, FacetResult>());
        }

        var map = tagFacets.ToDictionary(x => x.Value as string, x => x);
        var tags = tagInfos.ToDictionary(tagInfo => tagInfo, tagInfo => map.GetValueOrDefault(tagInfo.TagName));

        return new ReadOnlyDictionary<TagInfo, FacetResult>(tags);
    }

    private static string GetPagingFormatString(string searchTerm, IEnumerable<string> tags, SortOrder sortOrder, string routeName)
    {
        var result = $"/{routeName}?{SearchRequestPropertyNames.Page}={{0}}";

        void Append(string param, string name = null)
        {
            if (string.IsNullOrEmpty(param)) return;

            result += string.IsNullOrEmpty(name)
                ? $"&{param}"
                : $"&{name}={param}";
        }

        if (sortOrder != SortOrder.UpdatedLatest)
        {
            Append(((int)sortOrder).ToString(), SearchRequestPropertyNames.SortOrder);
        }
        Append(searchTerm, SearchRequestPropertyNames.Term);
        Append(string.Join('&', tags.Select(x => $"{SearchRequestPropertyNames.Tags}={x}")));
        return result;
    }

    private static string GetClearFiltersUri(SearchRequestModel request, string routeName)
    {
        var result = $"/{routeName}?{SearchRequestPropertyNames.Term}={request.Term}";
        if (request.SortOrder != SortOrder.UpdatedLatest)
        {
            result += $"&{SearchRequestPropertyNames.SortOrder}={(int)request.SortOrder}";
        }

        return result;
    }

    private static KeywordSearchQuery GetQuery(SearchRequestModel request, IEnumerable<string> validTags, SortOrder sortOrder, int pageSize)
    {
        var term = request.Term ?? string.Empty;
        term = term.Substring(0, Math.Min(term.Length, 255));
        var page = Math.Max(request.Page, 1);
        var filter = new Dictionary<string, IEnumerable<string>> { { "Tags", validTags } };

        var sortCategory = sortOrder switch
        {
            SortOrder.MostRelevant => SortCategory.Relevancy,
            _ => SortCategory.Updated,
        };

        var sortDirection = sortOrder switch
        {
            SortOrder.UpdatedOldest => SortDirection.Ascending,
            _ => SortDirection.Descending,
        };

        return new KeywordSearchQuery(term, page, pageSize, filter, sortCategory, sortDirection);
    }

    public async Task<ResourceSearchResultsViewModel> GetSearchModel(SearchRequestModel request, int pageSize, string routeName, CancellationToken cancellationToken)
    {
        // Kick off our page content query
        var rootPageTask = _resourcesRepository.FetchRootPageAsync(cancellationToken);

        // Get the available tags and validate the ones we've been given
        var tagInfos = await _resourcesRepository.GetSearchTagsAsync(cancellationToken);
        var validTags = request.Tags?.Where(x => tagInfos.Any(y => y.TagName == x)) ?? Array.Empty<string>();

        // Create our search query
        var query = GetQuery(request, validTags, request.SortOrder, pageSize);

        // Run the search and build our view model
        var searchResults = await _searchService.SearchResourcesAsync(query);
        var facetedTags = GetFacetedTags(tagInfos, searchResults.SearchResults.Facets);

        // Wait for the page content query to complete
        var pageContent = await rootPageTask;

        return new ResourceSearchResultsViewModel(
            pageContent,
            query.Keyword,
            searchResults.TotalCount,
            searchResults.TotalPages,
            searchResults.CurrentPage,
            searchResults.StartResultCount,
            searchResults.EndResultCount,
            searchResults.SearchResults.GetResults(),
            tagInfos,
            facetedTags,
            validTags,
            GetClearFiltersUri(request, routeName),
            GetPagingFormatString(query.Keyword, validTags, request.SortOrder, routeName),
            request.SortOrder);
    }
}
