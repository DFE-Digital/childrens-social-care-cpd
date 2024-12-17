using System.Runtime.CompilerServices;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class PathwaysModuleType 
{
    public const string IntroductoryModule = "Introductory Module";
    public const string RegularModule = "Regular Module";
}

public class PathwaysModule: IContent
{
    public string Name { get; set; }
    public string Type { get; set; }
    public Content OverviewPage { get; set; }
    public Content ContentsPage { get; set; }
    public Content DeclarationPage { get; set; }
    public Content CertificatePage { get; set; }
    public List<PathwaysModuleSection> Sections { get; set; }
    public CreditBlock CreditBlock { get; set; }
}
