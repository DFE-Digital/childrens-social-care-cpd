using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class AssetDownload : IContent
{
    public string LinkText { get; set; }
    public Asset Asset { get; set; }
}
