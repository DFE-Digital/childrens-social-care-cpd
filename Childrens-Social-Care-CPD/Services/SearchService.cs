using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Search;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD.Services;

internal class SearchService: ISearchService
{
    private readonly SearchClient _searchClient;
    private const int MinPageSize = 8;

    public SearchService(SearchClient searchClient)
    {
        _searchClient = searchClient;
    }

    private static string GetOrderBy(SortCategory category, SortDirection direction)
    {
        var orderBy = category switch
        {
            SortCategory.Created => "CreatedAt",
            SortCategory.Updated => "UpdatedAt",
            _ => "search.score()",
        };

        return direction switch
        {
            SortDirection.Ascending => $"{orderBy} asc",
            _ => $"{orderBy} desc",
        };
    }

    private static string GetFilter(IDictionary<string, IEnumerable<string>> filter)
    {
        IEnumerable<string> Formatter(KeyValuePair<string, IEnumerable<string>> kvp) =>
            kvp.Value.Select(value => $"{kvp.Key}/any(v: v eq '{value}')");
        var items = filter?.Select(kvp => string.Join(" or ", Formatter(kvp)));
        return string.Join("and", items ?? Array.Empty<string>());
    }

    public Task<SearchResourcesResult> SearchResourcesAsync(KeywordSearchQuery query)
    {
        var searchTerm = RemoveSpecialCharacters(query.Keyword);
        var skip = (Math.Max(query.Page, 1) - 1) * query.PageSize;
        var filter = GetFilter(query.Filter);
        var orderBy = GetOrderBy(query.SortCategory, query.SortDirection);
        var pageSize = Math.Max(query.PageSize, MinPageSize);

        var searchOptions = new SearchOptions()
        {
            QueryType = SearchQueryType.Simple,
            SearchMode = SearchMode.Any,
            IncludeTotalCount = true,
            SearchFields = { "Title", "Body" },
            HighlightFields = { "Body" },
            Facets = { "Tags,count:100" },
            Filter = string.IsNullOrEmpty(filter) ? null : filter,
            Size = pageSize,
            Skip = skip,
            OrderBy = { orderBy }
        };

        return _searchClient
            .SearchAsync<CpdDocument>(searchTerm, searchOptions)
            .ContinueWith(task =>
            {
                var searchResults = task.Result;
                var totalCount = searchResults.Value.TotalCount ?? 0;
                var totalPages = (long)Math.Ceiling((decimal)totalCount / pageSize);
                var currentPage = totalPages == 0 ? 0 : Math.Clamp(query.Page, 1, totalPages);
                var startResult = (currentPage - 1) * pageSize + 1;
                var endResult = Math.Min(startResult + pageSize - 1, totalCount);
                return new SearchResourcesResult(totalCount, totalPages, currentPage, startResult, endResult, searchResults.Value);
            });
    }

    private static string RemoveSpecialCharacters(string str)
    {
        return str == null
            ? null
            : Regex.Replace(str, "[^a-zA-Z0-9 ]+", "", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
    }
}