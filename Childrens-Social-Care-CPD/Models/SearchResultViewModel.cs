using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Search;

namespace Childrens_Social_Care_CPD.Models;

public record SearchResultViewModel(SearchResult<CpdDocument> SearchResult, IReadOnlyDictionary<string, TagInfo> TagMap);