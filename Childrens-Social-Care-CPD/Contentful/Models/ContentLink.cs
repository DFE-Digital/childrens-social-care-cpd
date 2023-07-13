using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ContentLink : IContent
{
    public string Name { get; set; }
    public string Uri { get; set; }
}
