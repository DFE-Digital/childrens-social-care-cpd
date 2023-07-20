using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class LinkCard : IContent
{
    public string Name { get; set; }
    public ContentLink TitleLink { get; set; }
    public string Text { get; set; }
}
