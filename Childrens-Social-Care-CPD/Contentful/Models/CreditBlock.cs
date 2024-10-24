using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class CreditBlock : IContent
{
    public Document DeveloperOfResource { get; set; }
    public Document SecondaryDevelopersOfResource { get; set; }
    
}
