using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Video : IContent
{
    public string Id { get; set; }
    public Asset VideoResource { get; set; }
    public Document Transcript {  get; set; }
}
