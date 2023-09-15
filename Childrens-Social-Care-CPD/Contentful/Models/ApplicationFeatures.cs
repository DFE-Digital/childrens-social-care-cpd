using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ApplicationFeatures : IContent
{
    public string Id { get; set; }
    public List<ApplicationFeature> Features { get; set; }
}