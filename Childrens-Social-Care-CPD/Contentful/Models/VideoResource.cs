using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class VideoResource : IContent
{
    public string Id { get; set; }
    public Asset Video { get; set; }
    public string Length { get; set; }
    public Document Transcript {  get; set; }
    public string EmbeddedSourceUrl { get; set; }
}
