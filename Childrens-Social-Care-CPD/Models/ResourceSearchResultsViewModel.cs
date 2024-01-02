using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.Search;

namespace Childrens_Social_Care_CPD.Models;

public record ResourceSearchResultsViewModel(
    Content PageContent,
    string SearchTerm,
    long TotalCount,
    long TotalPages,
    long CurrentPage,
    long StartResultCount,
    long EndResultCount,
    IEnumerable<TagInfo> Tags,
    IEnumerable<SearchResult<CpdDocument>> SearchResults,
    IReadOnlyDictionary<TagInfo, FacetResult> FacetedTags,
    IEnumerable<string> SelectedTags,
    string ClearFiltersUri,
    string PagingFormatString,
    SortOrder SortOrder);