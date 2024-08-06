using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ImageCard : IContent
{
    public string Id { get; set; }
    public Asset Image { get; set; }
    public Document Text { get; set; }
    public string TextPosition { get; set; }
}
