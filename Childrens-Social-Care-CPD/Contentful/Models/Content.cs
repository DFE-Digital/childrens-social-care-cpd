using Contentful.Core.Models;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class ContentTypes
{
    public const string Resource = "Resource";
}

public static class PageType
{
    public const string StandardPage = "Standard Page";
    public const string PathwaysOverviewPage = "Pathways: Overview Page";
    public const string PathwaysContentsPage = "Pathways: Table of Contents";
    public const string PathwaysTrainingContent = "Pathways: Training Content";
    public const string PathwaysDeclaration = "Pathways: Declaration Page";
    public const string PathwaysCertificate = "Pathways: Certificate Page";
    public const string AllPathwaysOverviewPage = "Pathways: All Pathways Overview Page";
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
    public bool ShowPrintThisPage { get; set; }
    public PathwaysModule PathwaysModule { get; set; }
    public string PageType { get; set; }

    [JsonProperty("$metadata")]
    public ContentfulMetadata Metadata { get; set; }
    public SystemProperties Sys { get; set; }

    public bool IsPathwaysPage
    {
        get
        {
            return PageType == Models.PageType.PathwaysCertificate
                || PageType == Models.PageType.PathwaysContentsPage
                || PageType == Models.PageType.PathwaysDeclaration
                || PageType == Models.PageType.PathwaysOverviewPage
                || PageType == Models.PageType.PathwaysTrainingContent
                || PageType == Models.PageType.AllPathwaysOverviewPage;
        }
    }
}