using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class NavigationMenu: IContent
{
    public string Name { get; set; }
    public List<ContentLink> Items { get; set; }
}
