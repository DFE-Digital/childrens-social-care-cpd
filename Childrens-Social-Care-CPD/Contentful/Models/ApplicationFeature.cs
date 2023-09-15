using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ApplicationFeature : IContent
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
}