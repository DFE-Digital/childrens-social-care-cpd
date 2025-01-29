using System.Runtime.CompilerServices;
using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class LocationInfo
{
    public string SectionName { get; set; }
    public int SectionNumber { get; set; }
    public int PageNumber { get; set; }
    public int TotalSections { get; set; }
}