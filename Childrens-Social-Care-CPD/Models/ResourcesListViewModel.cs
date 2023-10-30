using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.GraphQL.Queries;

namespace Childrens_Social_Care_CPD.Models;

public record ResourcesListViewModel(
    Content Content,
    SearchResourcesByTags.ResourceCollection Results,
    IEnumerable<TagInfo> TagInfos,
    int[] SelectedTags,
    int CurrentPage = 0,
    int TotalPages = 0,
    int TotalResults = 0,
    string PagingFormatString = "");
