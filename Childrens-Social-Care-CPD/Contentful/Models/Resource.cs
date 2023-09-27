using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Resource : IContent
{
    public string Id { get; set; }
    public string PreHeading { get; set; }
    public string Heading { get; set; }
    public string Summary { get; set; }
    public string ResourceListSummary { get; set; }
    public string From { get; set; }
    public string ResourceType { get; set; }
    public List<IContent> ResourceItems { get; set; }
}
