using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class AudioResource : IContent
{
    public string Id { get; set; }
    public Asset Audio { get; set; }
    public Document Transcript {  get; set; }
}
