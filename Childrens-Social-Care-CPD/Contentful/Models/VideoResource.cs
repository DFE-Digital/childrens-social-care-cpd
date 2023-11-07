using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class VideoResource : IContent
{
    public string Id { get; set; }
    public Asset Video { get; set; }
    public Document Transcript {  get; set; }
    public string EmbededTitle { get; set; }
    public string EmbededSourceUrl { get; set; }
}
