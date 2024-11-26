using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class QuoteAttributionAlignment
{
    public const string Left = "Left";
    public const string Centre = "Centre";
    public const string Right = "Right";
}

public class QuoteBox : IContent
{
    public string Name { get; set; }
    public Document QuoteText { get; set; }
    public Document Attribution { get ; set; }
    public string AttributionAlignment { get; set; }
}
