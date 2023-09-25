using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models;

public record ResourcesListViewModel(ContentfulCollection<Content> Content, IEnumerable<TagInfo> TagInfos, int[] SelectedTags, int CurrentPage = 0, int TotalPages = 0, int TotalResults = 0);
