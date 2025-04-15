using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class PromoBanner : IContent
{
    public string Title { get; set; }
    public string Text { get; set; }
    public ColumnLayout ColumnLayout { get; set; }
}
