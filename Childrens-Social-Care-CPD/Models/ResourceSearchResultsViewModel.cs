﻿using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Search;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Models;

[ExcludeFromCodeCoverage]
public record ResourceSearchResultsViewModel(
    string SearchTerm,
    long TotalCount,
    long TotalPages,
    long CurrentPage,
    long StartResultCount,
    long EndResultCount,
    IEnumerable<SearchResult<CpdDocument>> SearchResults,
    IEnumerable<TagInfo> Tags,
    IReadOnlyDictionary<TagInfo, FacetResult> FacetedTags,
    IEnumerable<string> SelectedTags,
    string ClearFiltersUri,
    string PagingFormatString,
    SortOrder SortOrder);