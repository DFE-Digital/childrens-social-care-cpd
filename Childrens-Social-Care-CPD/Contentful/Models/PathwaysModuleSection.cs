using System.Runtime.CompilerServices;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class PathwaysModuleSection: IContent
{
    public string Name { get; set; }
    public string ShortName {get; set; }
    public string Summary { get; set; }
    public List<Content> Pages { get; set; }
}
