using Azure.Search.Documents.Models;
using Azure;

namespace Childrens_Social_Care_CPD.Search;

public interface ISearchService
{
    Task<Response<SearchResults<CpdDocument>>> SearchResourcesAsync(KeywordSearchQuery query);
}