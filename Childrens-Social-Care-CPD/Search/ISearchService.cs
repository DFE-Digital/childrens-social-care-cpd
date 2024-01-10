namespace Childrens_Social_Care_CPD.Search;

public interface ISearchService
{
    Task<SearchResourcesResult> SearchResourcesAsync(KeywordSearchQuery query);
}