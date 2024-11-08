using Contentful.Core.Models;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class ContentTypes
{
    public const string Resource = "Resource";
}

public static class PrintThisPageLocations
{
    public const string BeforeFeedback = "Before Feedback";
    public const string BeforeCreditBlock = "Before Credit Block";
    public const string BottomOfPage = "Bottom of Page";
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
    public List<IContent> Items { get; set; }
    public NavigationMenu Navigation { get; set; }
    public RelatedContent RelatedContent { get; set; }
    public int? EstimatedReadingTime { get; set; }
    public List<Content> ParentPages { get; set; }
    public string BreadcrumbText { get; set; }
    public string PrintThisPageLocation { get; set; }

    [JsonProperty("$metadata")]
    public ContentfulMetadata Metadata { get; set; }
    public SystemProperties Sys { get; set; }
}