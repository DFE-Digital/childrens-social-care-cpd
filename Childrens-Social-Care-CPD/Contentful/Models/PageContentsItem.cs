using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class PageContentsItem : IContent
{
    public string ItemText { get; set; }
    public string AnchorLink { get; set; }
}