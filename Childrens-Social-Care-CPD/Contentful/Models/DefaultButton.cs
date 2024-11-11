using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class DefaultButton : IContent {
    public string ButtonText {get; set;}
    public string RelativeLink {get; set;}
}