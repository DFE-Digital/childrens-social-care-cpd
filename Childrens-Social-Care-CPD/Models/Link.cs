namespace Childrens_Social_Care_CPD.Models
{
    public class Link
    {
        public string LinkText { get; set; }
        public string LinkURL { get; set; }
        public string LinkSection { get; set; }
        public ContentPageType PageType { get; set; }
        public ContentPageName RedirectPageName { get; set; }

        public int SortOrder { get; set; }
    }
}
