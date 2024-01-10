using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Search;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD.Models;

[ExcludeFromCodeCoverage]
public record SearchResultViewModel(SearchResult<CpdDocument> SearchResult, IReadOnlyDictionary<string, TagInfo> TagMap);