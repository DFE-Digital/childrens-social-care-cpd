using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class Accordion : IContent
{
    public string Name { get; set; }
    public List<AccordionSection> Sections { get; set; }
}