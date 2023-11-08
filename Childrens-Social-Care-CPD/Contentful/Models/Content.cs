using Contentful.Core.Models;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class ContentTypes
{
    public const string Resource = "Resource";
}

public class Content : IContent
{
    public string Id { get; set; }
    public string ContentType { get; set; }
    public string Title { get; set; }
    public string ContentTitle { get; set; }
    public string ContentSubtitle { get; set; }
    public bool ShowContentHeader { get; set; }
    public string Category { get; set; }
    public ContentLink BackLink { get; set; }
    public SideMenu SideMenu { get; set; }
    public List<IContent> Items { get; set; }
    public RelatedContent RelatedContent { get; set; }

    public List<ContentLink> Navigation { get; set; }

    [JsonProperty("$metadata")]
    public ContentfulMetadata Metadata { get; set; }
    public SystemProperties Sys { get; set; }
}