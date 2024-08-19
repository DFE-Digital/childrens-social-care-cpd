using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Details : IContent
{
    public string SummaryText { get; set; }
    public string DetailsText { get; set; }
}
