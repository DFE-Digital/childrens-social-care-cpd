using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class ContentLinkIcon
{
    public const string SignpostIcon = "Signpost icon";
    public const string CompassIcon = "Compass icon";
}

public class ContentLink : IContent
{
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Icon {get; set; }
}
