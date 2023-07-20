using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Content : IContent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public SideMenu SideMenu { get; set; }
    public List<IContent> Items { get; set; }
}