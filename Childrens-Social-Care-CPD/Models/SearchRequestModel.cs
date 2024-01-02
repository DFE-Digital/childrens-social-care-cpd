namespace Childrens_Social_Care_CPD.Models;

public enum SortOrder
{
    UpdatedLatest,
    UpdatedOldest,
    MostRelevant,
}

public record SearchRequestModel(
    string Term,
    string[] Tags,
    int Page = 1,
    SortOrder SortOrder = SortOrder.UpdatedLatest);