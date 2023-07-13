using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class TextBlock : IContent
{
    public string Title { get; set; }
    public bool DisplayTitle { get; set; }
    public int TitleLevel { get; set; }
    public string Text { get; set; }
}
