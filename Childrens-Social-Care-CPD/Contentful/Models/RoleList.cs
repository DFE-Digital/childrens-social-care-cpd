using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class RoleList : IContent
{
    public string Title { get; set; }
    public List<Content> Roles { get; set; }
}
