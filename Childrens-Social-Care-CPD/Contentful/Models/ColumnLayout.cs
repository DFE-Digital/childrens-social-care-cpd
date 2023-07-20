using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class ColumnLayout : IContent
{
    public string Name { get; set; }
    public int ColumnCount { get; set; }
    public List<IContent> Items { get; set; }
}
