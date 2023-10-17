using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ImageResource : IContent
{
    public string Id { get; set; }
    public Asset Image { get; set; }
}
