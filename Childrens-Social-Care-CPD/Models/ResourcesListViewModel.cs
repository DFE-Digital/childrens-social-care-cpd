using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.GraphQL.Queries;

namespace Childrens_Social_Care_CPD.Models;

public static class ResourceSort
{
    public const string Updatednewest = "Updated (newest)";
    public const string Updatedoldest = "Updated (oldest)";
    public const string MostViewed = "Most viewed";
}


public record ResourcesListViewModel(
    Content Content,
    SearchResourcesByTags.ResourceCollection Results,
    IEnumerable<TagInfo> TagInfos,
    IEnumerable<string> SelectedTags,
    int SortOrder = 0,
    int CurrentPage = 0,
    int TotalPages = 0,
    int TotalResults = 0,
    string PagingFormatString = "");
