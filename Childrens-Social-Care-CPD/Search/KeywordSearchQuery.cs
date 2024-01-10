namespace Childrens_Social_Care_CPD.Search;

public record KeywordSearchQuery(
    string Keyword,
    int Page = 1,
    int PageSize = 8,
    IDictionary<string, IEnumerable<string>> Filter = null,
    SortCategory SortCategory = SortCategory.Relevancy,
    SortDirection SortDirection = SortDirection.Descending);