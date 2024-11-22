using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class QuoteBox : IContent
{
    public string Name { get; set; }
    public Document QuoteText { get; set; }
}
