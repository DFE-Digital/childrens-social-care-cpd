using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class LinkListCard : IContent
{
    public string Title { get; set; }
    public List<ContentLink> Links { get; set; }
}
