using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class AccordionSection : IContent
{
    public string Name { get; set; }
    public string Heading { get; set; }
    public string SummaryLine { get; set; }
    public List<IContent> Content { get; set; }
}