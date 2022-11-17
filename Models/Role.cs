namespace Childrens_Social_Care_CPD.Models
{
    public class Role
    {
        public string RolePageName { get; set; }
        public string RoleHeader { get; set; }
        public string RoleHeaderURL { get; set; }
        public string RoleContents { get; set; }
        public string RoleDescription { get; set; }
        public int SortOrder { get; set; }
        public ContentPageType PageType { get; set; }
        public RedirectPageName RedirectPageName { get; set; }
        public ContentPageName PageName { get; set; }
    }
}
