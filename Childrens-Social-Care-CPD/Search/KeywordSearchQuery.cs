namespace Childrens_Social_Care_CPD.Search;

public class KeywordSearchQuery
{
    public string Keyword { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; } = 8;
    public IDictionary<string, IEnumerable<string>> Filter { get; set; }
    public SortCategory SortCategory { get; set; } = SortCategory.Relevancy;
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
}