using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class PageContents : IContent
{
    public string Name { get; set; }
    public string DisplayText { get; set; }
    public List<PageContentsItem> ContentLinks { get; set; }
}