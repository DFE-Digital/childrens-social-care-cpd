using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class EmbeddedVideo : IContent
{
    public string Name { get; set; }
    public string EmbeddingCode { get; set; }
}