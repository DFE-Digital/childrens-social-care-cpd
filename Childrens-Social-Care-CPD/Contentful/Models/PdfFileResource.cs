using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class PdfFileResource : IContent
{
    public string Id { get; set; }
    public Asset File { get; set; }
    public Asset FileThumbnail { get; set; }
    public string FileDescription { get; set; }
    public int PageCount { get; set; }
}
