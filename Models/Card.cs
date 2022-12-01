namespace Childrens_Social_Care_CPD.Models
{
    public class Card
    {
        public string CardHeader { get; set; }
        public string CardContents { get; set; }
        public string CardDescription { get; set; }
        public int SortOrder { get; set; }
        public ContentPageType PageType { get; set; }
        public RedirectPageName RedirectPageName { get; set; }
    }
}
