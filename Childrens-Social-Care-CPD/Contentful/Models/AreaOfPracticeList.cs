using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class AreaOfPracticeList : IContent
{
    public string Title { get; set; }
    public List<Content> Areas { get; set; }
}
