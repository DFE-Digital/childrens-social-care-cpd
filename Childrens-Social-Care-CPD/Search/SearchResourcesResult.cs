using Azure.Search.Documents.Models;

namespace Childrens_Social_Care_CPD.Search;

public record SearchResourcesResult(
    long TotalCount,
    long TotalPages,
    long CurrentPage,
    long StartResultCount,
    long EndResultCount,
    SearchResults<CpdDocument> SearchResults);
