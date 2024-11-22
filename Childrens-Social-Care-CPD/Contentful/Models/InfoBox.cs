using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public static class InfoBoxType
{
    public const string BlueI = "Blue \"i\" icon";
    public const string GreenI = "Green \"i\" icon";
    public const string GreenBrain = "Green \"brain\" icon";
}

public class InfoBox : IContent
{
    public string Title { get; set; }
    public bool DisplayTitle { get; set; }
    public int TitleLevel { get; set; }
    public Document Document { get; set; }
    public string Type { get; set; }
}
