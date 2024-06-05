using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class InfoBox : IContent
{
    public string Title { get; set; }
    public bool DisplayTitle { get; set; }
    public int TitleLevel { get; set; }
    public Document Document { get; set; }
}
