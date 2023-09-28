using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models;

public record ResourcesListViewModel(
    Content Content,
    ContentfulCollection<Resource> SearchResults,
    IEnumerable<TagInfo> TagInfos,
    int[] SelectedTags,
    int CurrentPage = 0,
    int TotalPages = 0,
    int TotalResults = 0,
    string PagingFormatString = "");
