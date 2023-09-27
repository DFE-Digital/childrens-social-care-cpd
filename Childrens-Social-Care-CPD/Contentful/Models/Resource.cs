using Contentful.Core.Models;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Resource : IContent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string From { get; set; }
    public List<string> Type { get; set; }
    public string Summary { get; set; }
    public string SearchSummary { get; set; }
    public List<IContent> Items { get; set; }

    // need these for queries
    [JsonProperty("$metadata")]
    public ContentfulMetadata Metadata { get; set; }
    public SystemProperties Sys { get; set; }
}
